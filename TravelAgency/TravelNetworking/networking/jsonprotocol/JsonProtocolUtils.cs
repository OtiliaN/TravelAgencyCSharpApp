using System;
using System.Collections.Generic;
using TravelModel.domain;

namespace TravelNetworking.networking.jsonprotocol
{
    public static class JsonProtocolUtils
    {
        // Create a login request
        public static Request CreateLoginRequest(string username, string password)
        {
            return new Request
            {
                RequestType = RequestType.Login,
                Username = username,
                Password = password
            };
        }

        // Create a logout request
        public static Request CreateLogoutRequest(Agent agent)
        {
            return new Request
            {
                RequestType = RequestType.Logout,
                Agent = agent
            };
        }

        // Create a request to get all flights
        public static Request CreateGetAllFlightsRequest()
        {
            return new Request
            {
                RequestType = RequestType.GetAllFlights
            };
        }

        // Create a request to get flights by destination and date
        public static Request CreateGetFlightsByDestinationRequest(string destination, DateTime departureDate, TimeSpan startTime, TimeSpan endTime)
        {
            return new Request
            {
                RequestType = RequestType.GetFlightsByDestinationAndDate,
                Destination = destination,
                DepartureDate = departureDate,
                StartTime = startTime,
                EndTime = endTime
            };
        }

        // Create a request to buy tickets
        public static Request CreateBuyTicketsRequest(Flight flight, List<string> passengers, int numberOfSeats)
        {
            return new Request
            {
                RequestType = RequestType.BuyTickets,
                Flight = flight,
                Passengers = passengers,
                NumberOfSeats = numberOfSeats
            };
        }

        // Create a successful response
        public static Response CreateOkResponse()
        {
            return new Response
            {
                ResponseType = ResponseType.OK
            };
        }

        // Create an error response
        public static Response CreateErrorResponse(string errorMessage)
        {
            return new Response
            {
                ResponseType = ResponseType.Error,
                ErrorMessage = errorMessage
            };
        }

        // Create a login response
        public static Response CreateLoginResponse(Agent agent)
        {
            return new Response
            {
                ResponseType = ResponseType.Login,
                Agent = agent
            };
        }

        // Create a response for getting all flights
        public static Response CreateGetAllFlightsResponse(IEnumerable<Flight> flights)
        {
            return new Response
            {
                ResponseType = ResponseType.GetAllFlights,
                Flights = flights
            };
        }

        // Create a response for getting flights by destination and date
        public static Response CreateGetFlightsByDestinationResponse(IEnumerable<Flight> flights)
        {
            return new Response
            {
                ResponseType = ResponseType.GetFlightsByDestinationAndDate,
                Flights = flights
            };
        }

        // Create a response for a modified flight
        public static Response CreateFlightModifiedResponse(Flight flight)
        {
            return new Response
            {
                ResponseType = ResponseType.ModifiedFlight,
                Flight = flight
            };
        }
    }
}