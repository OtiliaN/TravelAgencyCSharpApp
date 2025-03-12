namespace AgentieTurismCSharp.domain;

public class Booking : Entity<long>
{
    public Flight Flight { get; set; }
    public List<string> Passengers { get; set; }
    public int NumberOfSeats { get; set; }

    public Booking(Flight flight, List<string> passengers, int numberOfSeats)
    {
        Flight = flight;
        Passengers = passengers;
        NumberOfSeats = numberOfSeats;
    }
}