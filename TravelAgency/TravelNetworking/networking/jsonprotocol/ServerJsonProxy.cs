using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using log4net;
using TravelModel.domain;
using TravelServices.service;

namespace TravelNetworking.networking.jsonprotocol
{
    public class ServerJsonProxy : IService
    {
        private readonly string _host;
        private readonly int _port;

        private IObserver _client;
        private NetworkStream _stream;
        private TcpClient _connection;
        private volatile bool _finished;
        private EventWaitHandle _waitHandle;
        private Queue<Response> _responses;

        private static readonly ILog logger = LogManager.GetLogger(typeof(ServerJsonProxy));

        public ServerJsonProxy(string host, int port)
        {
            _host = host;
            _port = port;
            _responses = new Queue<Response>();
        }

        private void InitializeConnection()
        {
            try
            {
                _connection = new TcpClient(_host, _port);
                _stream = _connection.GetStream();
                _finished = false;
                _waitHandle = new AutoResetEvent(false);
                StartReader();
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace);
            }
        }

        private void StartReader()
        {
            var thread = new Thread(Run);
            thread.Start();
        }

        public void Run()
        {
            using var streamReader = new StreamReader(_stream, Encoding.UTF8);
            while (!_finished)
            {
                try
                {
                    string responseJson = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(responseJson))
                    {
                        logger.Error("Received empty response");
                        continue;
                    }

                    var response = JsonSerializer.Deserialize<Response>(responseJson);
                    if (IsUpdate(response))
                    {
                        HandleUpdate(response);
                    }
                    else
                    {
                        lock (_responses)
                        {
                            _responses.Enqueue(response);
                        }

                        _waitHandle.Set();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Error reading response", ex);
                }
            }
        }

        private bool IsUpdate(Response response)
        {
            return response.ResponseType == ResponseType.ModifiedFlight;
        }

        private void HandleUpdate(Response response)
        {
            if (response.ResponseType == ResponseType.ModifiedFlight)
            {
                var flight = response.Flight;
                try
                {
                    _client.flightModified(flight);
                }
                catch (Exception ex)
                {
                    logger.Error("Error modifying trip", ex);
                    logger.Error(ex.StackTrace);
                }
            }
        }

        private void SendRequest(Request request)
        {
            try
            {
                lock (_stream)
                {
                    string jsonReq = JsonSerializer.Serialize(request);
                    byte[] buffer = Encoding.UTF8.GetBytes(jsonReq + "\n");
                    _stream.Write(buffer, 0, buffer.Length);
                    _stream.Flush();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error sending request: " + ex.Message);
            }
        }

        private Response ReadResponse()
        {
            _waitHandle.WaitOne();
            lock (_responses)
            {
                return _responses.Dequeue();
            }
        }

        public Agent Login(string username, string password, IObserver client)
        {
            InitializeConnection();
            var request = JsonProtocolUtils.CreateLoginRequest(username, password);
            SendRequest(request);
            var response = ReadResponse();
            if (response.ResponseType == ResponseType.Error)
            {
                CloseConnection();
                throw new Exception("Error logging in: " + username);
            }

            _client = client;
            return response.Agent;
        }

        public void Logout(Agent agent, IObserver client)
        {
            var request = JsonProtocolUtils.CreateLogoutRequest(agent);
            SendRequest(request);
            var response = ReadResponse();
            if (response.ResponseType == ResponseType.Error)
            {
                CloseConnection();
                throw new Exception("Error logging out");
            }

            if (response.ResponseType == ResponseType.OK)
            {
                CloseConnection();
            }
        }

        public IEnumerable<Flight> GetAllFlights()
        {
            var request = JsonProtocolUtils.CreateGetAllFlightsRequest();
            SendRequest(request);
            var response = ReadResponse();
            if (response.ResponseType == ResponseType.Error)
            {
                throw new Exception("Error getting all flights");
            }

            return response.Flights;
        }

        public IEnumerable<Flight> GetFlightsByDestinationAndDate(string destination, DateTime departureDate)
        {
            logger.Info($"Getting flights for destination {destination} on date {departureDate}");
            var request = JsonProtocolUtils.CreateGetFlightsByDestinationRequest(destination, departureDate, TimeSpan.Zero, TimeSpan.Zero);
            SendRequest(request);
            var response = ReadResponse();

            if (response.ResponseType == ResponseType.Error)
            {
                throw new Exception("Error getting flights by destination and date: " + response.ErrorMessage);
            }

            return response.Flights;
        }

        public bool BuyTickets(Flight flight, List<string> passengers, int numberOfSeats)
        {
            logger.Info($"Buying {numberOfSeats} tickets for flight {flight.Id}");
            var request = JsonProtocolUtils.CreateBuyTicketsRequest(flight, passengers, numberOfSeats);
            SendRequest(request);
            var response = ReadResponse();

            if (response.ResponseType == ResponseType.Error)
            {
                throw new Exception("Error buying tickets: " + response.ErrorMessage);
            }

            return true;
        }

        private void CloseConnection()
        {
            _finished = true;
            try
            {
                _stream.Close();
                _connection.Close();
                _waitHandle.Close();
                _client = null;
            }
            catch (Exception ex)
            {
                logger.Error("Error closing connection", ex);
            }
        }
    }
}