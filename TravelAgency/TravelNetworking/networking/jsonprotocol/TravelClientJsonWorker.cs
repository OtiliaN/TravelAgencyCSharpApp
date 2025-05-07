using System;
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
    public class TravelClientJsonWorker : IObserver
    {
        private readonly IService _server;
        private readonly TcpClient _connection;

        private readonly NetworkStream _networkStream;
        private volatile bool _connected;
        private static readonly ILog logger = LogManager.GetLogger(typeof(TravelClientJsonWorker));

        public TravelClientJsonWorker(IService server, TcpClient connection)
        {
            _server = server;
            _connection = connection;
            try
            {
                _networkStream = connection.GetStream();
                _connected = true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        public virtual void Run()
        {
            using var streamReader = new StreamReader(_connection.GetStream(), Encoding.UTF8);
            while (_connected)
            {
                try
                {
                    string requestJson = streamReader.ReadLine();
                    if (string.IsNullOrEmpty(requestJson))
                    {
                        logger.Error("Received empty request");
                        continue;
                    }

                    logger.DebugFormat("Received request: {0}", requestJson);
                    var request = JsonSerializer.Deserialize<Request>(requestJson);
                    var response = HandleRequest(request);
                    if (response != null)
                    {
                        SendResponse(response);
                    }
                }
                catch (Exception ex)
                {
                    logger.ErrorFormat("Error processing request: {0}", ex.Message);
                    if (ex.InnerException != null)
                    {
                        logger.ErrorFormat("Inner exception: {0}", ex.InnerException.Message);
                    }

                    logger.Error(ex.StackTrace);
                }

                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    logger.Error("Error sleeping thread: " + ex.Message);
                }
            }
        }

        private static readonly Response OkResponse = JsonProtocolUtils.CreateOkResponse();

        private void SendResponse(Response response)
        {
            var jsonStr = JsonSerializer.Serialize(response);
            logger.DebugFormat("Sending response: {0}", jsonStr);
            lock (_networkStream)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(jsonStr + "\n");
                _networkStream.Write(buffer, 0, buffer.Length);
                _networkStream.Flush();
            }
        }

        public void flightModified(Flight flight)
        {
            var response = JsonProtocolUtils.CreateFlightModifiedResponse(flight);
            logger.Debug("Sending flight modified response");
            try
            {
                SendResponse(response);
            }
            catch (IOException ex)
            {
                logger.Error("Error sending flight modified response: " + ex.Message);
            }
        }

        private Response HandleRequest(Request request)
        {
            switch (request.RequestType)
            {
                case RequestType.Login:
                    return HandleLoginRequest(request);
                case RequestType.Logout:
                    return HandleLogoutRequest(request);
                case RequestType.GetAllFlights:
                    return HandleGetAllFlightsRequest();
                case RequestType.GetFlightsByDestinationAndDate:
                    return HandleGetFlightsByDestinationAndDateRequest(request);
                case RequestType.BuyTickets:
                    return HandleBuyTicketsRequest(request);
                default:
                    return JsonProtocolUtils.CreateErrorResponse("Unknown request type");
            }
        }

        private Response HandleLoginRequest(Request request)
        {
            logger.DebugFormat("Received login request");
            try
            {
                var agent = _server.Login(request.Username, request.Password, this);
                return JsonProtocolUtils.CreateLoginResponse(agent);
            }
            catch (Exception ex)
            {
                _connected = false;
                return JsonProtocolUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Response HandleLogoutRequest(Request request)
        {
            logger.DebugFormat("Received logout request");
            try
            {
                _server.Logout(request.Agent, this);
                _connected = false;
                return OkResponse;
            }
            catch (Exception ex)
            {
                return JsonProtocolUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Response HandleGetAllFlightsRequest()
        {
            logger.DebugFormat("Received get all flights request");
            try
            {
                var flights = _server.GetAllFlights();
                return JsonProtocolUtils.CreateGetAllFlightsResponse(flights);
            }
            catch (Exception ex)
            {
                return JsonProtocolUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Response HandleGetFlightsByDestinationAndDateRequest(Request request)
        {
            logger.DebugFormat("Received get flights by destination and date request");
            try
            {
                var flights = _server.GetFlightsByDestinationAndDate(request.Destination, request.DepartureDate);
                return JsonProtocolUtils.CreateGetFlightsByDestinationResponse(flights);
            }
            catch (Exception ex)
            {
                return JsonProtocolUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Response HandleBuyTicketsRequest(Request request)
        {
            logger.DebugFormat("Received buy tickets request");
            try
            {
                var success = _server.BuyTickets(request.Flight, request.Passengers, request.NumberOfSeats);
                return success ? OkResponse : JsonProtocolUtils.CreateErrorResponse("Failed to buy tickets");
            }
            catch (Exception ex)
            {
                return JsonProtocolUtils.CreateErrorResponse(ex.Message);
            }
        }
    }
}