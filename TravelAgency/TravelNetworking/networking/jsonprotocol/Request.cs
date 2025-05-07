using TravelModel.domain;

namespace TravelNetworking.networking.jsonprotocol
{
    public class Request
    {
        public RequestType RequestType { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public Agent Agent { get; set; }

        public Booking Booking { get; set; }
        
        public Flight Flight { get; set; } 
        public List<string> Passengers { get; set; } 
        public int NumberOfSeats { get; set; } 

        public string Destination { get; set; }
        public DateTime DepartureDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public long Id { get; set; }
    }
}
