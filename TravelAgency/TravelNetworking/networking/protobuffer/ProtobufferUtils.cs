using Org.Protocol;
using Agent = TravelModel.domain.Agent;
using Flight = TravelModel.domain.Flight;
using Booking = TravelModel.domain.Booking;


namespace TravelNetworking.networking.protobuffer
{
    public static class ProtobufferUtils
    {
        // Create a successful response
        public static Response CreateOkResponse()
        {
            return new Response
            {
                ResponseType = ResponseType.ResponseOk
            };
        }

        // Create an error response
        public static Response CreateErrorResponse(string errorMessage)
        {
            return new Response
            {
                ResponseType = ResponseType.ResponseError,
                ErrorMessage = errorMessage
            };
        }

        // Create a login response
        public static Response CreateLoginResponse(Agent agent)
        {
            Org.Protocol.Agent protoAgent = new Org.Protocol.Agent
            {
                Id = agent.Id,
                Name = agent.Name,
                Password = agent.Password
            };
            return new Response
            {
                ResponseType = ResponseType.ResponseOk,
                Agent = protoAgent
            };
        }

        // Create a response for getting all flights
        public static Response CreateGetAllFlightsResponse(IEnumerable<Flight> flights)
        {
            var flightList = new List<Org.Protocol.Flight>();
            foreach (var flight in flights)
            {
                Org.Protocol.Flight protoFlight = new Org.Protocol.Flight
                {
                    Id = flight.Id,
                    Destination = flight.Destination,
                    DepartureDateTime = flight.DepartureDateTime.ToString("o"), // ISO 8601 format
                    Airport = flight.Airport,
                    AvailableSeats = flight.AvailableSeats
                };
                flightList.Add(protoFlight);
            }

            return new Response
            {
                ResponseType = ResponseType.ResponseGetAllFlights,
                Flights = { flightList }
            };
        }

        // Create a response for getting flights by destination and date
        public static Response CreateGetFlightsByDestinationResponse(IEnumerable<Flight> flights)
        {
            var flightList = new List<Org.Protocol.Flight>();
            foreach (var flight in flights)
            {
                Org.Protocol.Flight protoFlight = new Org.Protocol.Flight
                {
                    Id = flight.Id,
                    Destination = flight.Destination,
                    DepartureDateTime = flight.DepartureDateTime.ToString("o"), // ISO 8601 format
                    Airport = flight.Airport,
                    AvailableSeats = flight.AvailableSeats
                };
                flightList.Add(protoFlight);
            }

            return new Response
            {
                ResponseType = ResponseType.ResponseGetFlightsByDestinationAndDate,
                Flights = { flightList }
            };
        }

        // Create a response for a modified flight
        public static Response CreateFlightModifiedResponse(Flight flight)
        {
            Org.Protocol.Flight protoFlight = new Org.Protocol.Flight
            {
                Id = flight.Id,
                Destination = flight.Destination,
                DepartureDateTime = flight.DepartureDateTime.ToString("o"), // ISO 8601 format
                Airport = flight.Airport,
                AvailableSeats = flight.AvailableSeats
            };

            return new Response
            {
                ResponseType = ResponseType.ResponseModifiedFlight,
                Flight = protoFlight
            };
        }
        
        public static Agent GetAgent(Org.Protocol.Agent agentProto)
        {
            Agent agent = new Agent(agentProto.Name, agentProto.Password);
            agent.Id = agentProto.Id;
            return agent;
        }

        public static Flight GetFlight(Org.Protocol.Flight flightProto)
        {
            if (flightProto == null)
            {
                throw new ArgumentNullException(nameof(flightProto), "FlightProto cannot be null.");
            }

            if (string.IsNullOrEmpty(flightProto.DepartureDateTime))
            {
                throw new ArgumentException("DepartureDateTime cannot be null or empty.");
            }

            if (flightProto.Id <= 0)
            {
                throw new ArgumentException($"Invalid Flight ID: {flightProto.Id}");
            }

            // Convert the DepartureDateTime string to a DateTime object
            DateTime departureDateTime = Convert.ToDateTime(flightProto.DepartureDateTime);

            // Create a new Flight object using the mapped values
            Flight flight = new Flight(flightProto.Destination, departureDateTime, flightProto.Airport, flightProto.AvailableSeats)
            {
                Id = flightProto.Id // Setăm Id-ul
            };

            return flight;
        }
    }
}