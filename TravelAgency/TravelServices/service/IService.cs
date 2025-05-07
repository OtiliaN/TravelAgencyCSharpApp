using TravelModel.domain;

namespace TravelServices.service;


public interface IService
{
    Agent Login(string username, string password, IObserver client);
    IEnumerable<Flight> GetAllFlights();
    IEnumerable<Flight> GetFlightsByDestinationAndDate(string destination, DateTime departureDate);
    bool BuyTickets(Flight flight, List<string> passengers, int numberOfSeats);
    
    void Logout(Agent agent, IObserver client);
}