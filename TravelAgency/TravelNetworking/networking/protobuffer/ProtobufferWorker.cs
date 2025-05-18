using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using log4net;
using Org.Protocol;
using TravelServices.service;

namespace TravelNetworking.networking.protobuffer
{
    public class ProtobufferWorker : IObserver
    {
        private readonly IService _server;
        private readonly TcpClient _connection;

        private readonly NetworkStream _networkStream;
        private volatile bool _connected;
        private static readonly ILog logger = LogManager.GetLogger(typeof(ProtobufferWorker));

        public ProtobufferWorker(IService server, TcpClient connection)
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
            while (_connected)
            {
                try
                {
                    var request = Org.Protocol.Request.Parser.ParseDelimitedFrom(_networkStream);
                    if (request == null)
                    {
                        logger.Error("Received null request");
                        continue;
                    }

                    logger.DebugFormat("Received request: {0}", request.RequestType);
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

        private static readonly Org.Protocol.Response OkResponse = new Org.Protocol.Response
        {
            ResponseType = Org.Protocol.ResponseType.ResponseOk
        };

        private void SendResponse(Org.Protocol.Response response)
        {
            lock (_networkStream)
            {
                response.WriteDelimitedTo(_networkStream);
                _networkStream.Flush();
                logger.DebugFormat("Sent response: {0}", response.ResponseType);
            }
        }

        public void flightModified(TravelModel.domain.Flight flight)
        {
            var response = ProtobufferUtils.CreateFlightModifiedResponse(flight);
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

        private Org.Protocol.Response HandleRequest(Org.Protocol.Request request)
        {
            switch (request.RequestType)
            {
                case Org.Protocol.RequestType.RequestLogin:
                    return HandleLoginRequest(request);
                case Org.Protocol.RequestType.RequestLogout:
                    return HandleLogoutRequest(request);
                case Org.Protocol.RequestType.RequestGetAllFlights:
                    return HandleGetAllFlightsRequest();
                case Org.Protocol.RequestType.RequestGetFlightsByDestinationAndDate:
                    return HandleGetFlightsByDestinationAndDateRequest(request);
                case Org.Protocol.RequestType.RequestBuyTickets:
                    return HandleBuyTicketsRequest(request);
                default:
                    return ProtobufferUtils.CreateErrorResponse("Unknown request type");
            }
        }

        private Org.Protocol.Response HandleLoginRequest(Org.Protocol.Request request)
        {
            logger.DebugFormat("Received login request");
            try
            {
                var agent = _server.Login(request.Username, request.Password, this);
                return ProtobufferUtils.CreateLoginResponse(agent);
            }
            catch (Exception ex)
            {
                _connected = false;
                return ProtobufferUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Org.Protocol.Response HandleLogoutRequest(Org.Protocol.Request request)
        {
            logger.DebugFormat("Received logout request");
            try
            {
                var agent = ProtobufferUtils.GetAgent(request.Agent);
                _server.Logout(agent, this);
                _connected = false;
                return OkResponse;
            }
            catch (Exception ex)
            {
                return ProtobufferUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Org.Protocol.Response HandleGetAllFlightsRequest()
        {
            logger.DebugFormat("Received get all flights request");
            try
            {
                var flights = _server.GetAllFlights();
                return ProtobufferUtils.CreateGetAllFlightsResponse(flights);
            }
            catch (Exception ex)
            {
                return ProtobufferUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Org.Protocol.Response HandleGetFlightsByDestinationAndDateRequest(Org.Protocol.Request request)
        {
            logger.DebugFormat("Received get flights by destination and date request");
            try
            {
                var departureDate = DateTime.Parse(request.DepartureDate);
                var flights = _server.GetFlightsByDestinationAndDate(request.Destination, departureDate);
                return ProtobufferUtils.CreateGetFlightsByDestinationResponse(flights);
            }
            catch (Exception ex)
            {
                return ProtobufferUtils.CreateErrorResponse(ex.Message);
            }
        }

        private Org.Protocol.Response HandleBuyTicketsRequest(Org.Protocol.Request request)
        {
            logger.DebugFormat("Received buy tickets request");
            try
            {
                if (request.Booking == null)
                {
                    logger.Error("Booking is null in buy tickets request.");
                    return ProtobufferUtils.CreateErrorResponse("Booking cannot be null.");
                }

                if (request.Booking.Flight == null)
                {
                    logger.Error("Flight is null in booking.");
                    return ProtobufferUtils.CreateErrorResponse("Flight cannot be null.");
                }

                if (request.Booking.Passengers == null || request.Booking.Passengers.Count == 0)
                {
                    logger.Error("Passengers list is null or empty in booking.");
                    return ProtobufferUtils.CreateErrorResponse("Passengers list cannot be null or empty.");
                }

                if (request.Booking.NumberOfSeats <= 0)
                {
                    logger.Error("Number of seats is invalid in booking.");
                    return ProtobufferUtils.CreateErrorResponse("Number of seats must be greater than 0.");
                }

                var flight = ProtobufferUtils.GetFlight(request.Booking.Flight);
                logger.DebugFormat("Flight details: Id={0}, Destination={1}, AvailableSeats={2}",
                    flight.Id, flight.Destination, flight.AvailableSeats);

                var success = _server.BuyTickets(flight, new List<string>(request.Booking.Passengers), request.Booking.NumberOfSeats);
                return success ? OkResponse : ProtobufferUtils.CreateErrorResponse("Failed to buy tickets");
            }
            catch (Exception ex)
            {
                logger.Error("Error in HandleBuyTicketsRequest: " + ex.Message);
                return ProtobufferUtils.CreateErrorResponse(ex.Message);
            }
        }
    }
}