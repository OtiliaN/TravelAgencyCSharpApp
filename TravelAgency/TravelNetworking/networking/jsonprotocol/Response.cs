using TravelModel.domain;

namespace TravelNetworking.networking.jsonprotocol;

public class Response
{
    public ResponseType ResponseType { get; set; }
    public string ErrorMessage { get; set; }
    public Agent Agent { get; set; }
    public Booking Booking { get; set; }
    public IEnumerable<Flight> Flights { get; set; }
    public IEnumerable<Booking> Bookings { get; set; }
    public Flight Flight { get; set; }
}