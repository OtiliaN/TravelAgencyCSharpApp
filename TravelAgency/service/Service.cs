using AgentieTurismCSharp.domain;
using TravelAgency.repository;
using TravelAgency.utils;

namespace TravelAgency.service;

public class Service
{
    private readonly IAgentRepository _agentRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IBookingRepository _bookingRepository;

    public Service(IAgentRepository agentRepository, IFlightRepository flightRepository, IBookingRepository bookingRepository)
    {
        _agentRepository = agentRepository;
        _flightRepository = flightRepository;
        _bookingRepository = bookingRepository;
    }

    public bool Login(string username, string password)
    {
        var agent = _agentRepository.FindByUsername(username);
        if (agent == null)
        {
            return false;
        }

        return PasswordUtils.VerifyPassword(password, agent.Password);
    }

    public IEnumerable<Flight> GetAllFlights()
    {
        return _flightRepository.FindAll();
    }
    
    public IEnumerable<Flight> SearchFlights(string destination, DateTime departureDate)
    {
        return _flightRepository.FindByDestinationAndDeparture(destination, departureDate);
    }

   
    public bool BuyTickets(Flight flight, List<string> passengers, int numberOfSeats)
    {
        if (flight == null)
        {
            throw new ArgumentNullException(nameof(flight), "Flight cannot be null");
        }

        if (passengers == null || passengers.Count == 0)
        {
            throw new ArgumentNullException(nameof(passengers), "Passengers list cannot be null or empty");
        }

        if (flight.AvailableSeats < numberOfSeats)
        {
            return false;
        }

        Booking booking = new Booking(flight, passengers, numberOfSeats);
        Booking savedBooking = _bookingRepository.Save(booking);

        if (savedBooking != null)
        {
            flight.AvailableSeats -= numberOfSeats;
            _flightRepository.Update(flight);
            return true;
        }

        return false;
    }

}