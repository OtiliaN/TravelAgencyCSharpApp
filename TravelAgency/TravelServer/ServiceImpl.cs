
using log4net;
using TravelModel.domain;
using TravelPersistence.exceptions;
using TravelPersistence.persistence.interfaces;
using TravelPersistence.persistence.utils;
using TravelServices.service;

namespace TravelServer;
public class ServiceImpl: IService
{
    private readonly IAgentRepository _agentRepository;
    private readonly IFlightRepository _flightRepository;
    private readonly IBookingRepository _bookingRepository;

    private readonly IDictionary<string, IObserver> loggedClients;
    private static readonly ILog logger = LogManager.GetLogger(typeof(ServiceImpl));

    public ServiceImpl(IAgentRepository agentRepository, IFlightRepository flightRepository, IBookingRepository bookingRepository)
    {
        _agentRepository = agentRepository;
        _flightRepository = flightRepository;
        _bookingRepository = bookingRepository;
        loggedClients = new Dictionary<string, IObserver>();

    }

    public Agent Login(string username, string password, IObserver client)
    {
        var agent = _agentRepository.FindByUsername(username);

        if (agent != null && PasswordUtils.VerifyPassword(password, agent.Password))
        {
            if (loggedClients.ContainsKey(username))
            {
                throw new InvalidOperationException("Agent already logged in!");
            }
            loggedClients[username] = client;
            return agent;
        }
        else
        {
            throw new Exception("Authentication failed!");
        }
    }


    public void Logout(Agent agent, IObserver client)
    {
        loggedClients.Remove(agent.Name);
        logger.Info("User logged out: " + agent.Name);
    }
    
    public IEnumerable<Flight> GetAllFlights()
    {
        logger.Info("Getting all flights");
        try
        {
            IEnumerable<Flight> result = _flightRepository.FindAll() ?? throw new ServiceException("Failed to get all trips");
            logger.Info("Trips retrieved successfully " + result);
            List<Flight> trips = result.ToList();
            return trips;
        }
        catch (RepositoryException e)
        {
            logger.Error("Failed to get all trips", e);
            throw new ServiceException("Failed to get all trips", e);
        }
    }
    
    public IEnumerable<Flight> GetFlightsByDestinationAndDate(string destination, DateTime departureDate)
    {
        try
        {
            return _flightRepository.FindByDestinationAndDeparture(destination, departureDate);
        }
        catch (Exception ex)
        {
            throw new Exception("Could not find flights", ex);
        }
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
            notifyLoggedClients(flight);
            return true;
        }

        return false;
    }
    
    private void notifyLoggedClients(Flight flight)
    {
        foreach (var client in loggedClients)
        {
            IObserver clientObserver = client.Value;
            Task.Run(() =>
            {
                try
                {
                    clientObserver.flightModified(flight);
                }
                catch (Exception ex)
                {
                    logger.Error($"Failed to notify client {client.Key}: {ex.Message}", ex);
                }
            });
        }
    }

}