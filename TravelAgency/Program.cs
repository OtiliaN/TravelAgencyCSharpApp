// See https://aka.ms/new-console-template for more information

using System.Configuration;
using AgentieTurismCSharp.domain;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;
using TravelAgency.repository;
using TravelAgency.repository.impl;

namespace TravelAgency
{
    
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static string GetConnectionStringByName(string name)
        {
            // Assume failure.
            string returnValue = null;

            // Look for the name in the connectionStrings section.
            ConnectionStringSettings settings =ConfigurationManager.ConnectionStrings[name];

            // If found, return the connection string.
            if (settings != null)
                returnValue = settings.ConnectionString;

            return returnValue;
        }

        static void Main(string[] args)
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log4net.config");
            FileInfo fileInfo;

            try
            {
                fileInfo = new FileInfo(configPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        
            XmlConfigurator.Configure(fileInfo);


            IDictionary<String, String> props = new Dictionary<String, String>();
            props.Add("connectionString", GetConnectionStringByName("travel_agency"));

            // Create an instance of AgentRepositoryImpl
            IAgentRepository agentRepository = new AgentRepositoryImpl(props);
            IFlightRepository flightRepository = new FlightRepositoryImpl(props);
            IBookingRepository bookingRepository = new BookingRepositoryImpl(props);

            
            /*Tests for repo's CRUD functions*/
            TestAgentRepo(agentRepository);
            TestFlightRepo(flightRepository);
            TestBookingRepo(bookingRepository);
        }
        private static void TestAgentRepo(IAgentRepository agentRepository){
            /*FIND ONE*/
            long agentId = 1;
            Agent agent = agentRepository.FindOne(agentId);
            
            if (agent != null)
            {
                Console.WriteLine($"Agent found: {agent.Name}, {agent.Password}");
            }
            else
            {
                Console.WriteLine("Agent not found.");
            }
            
            /*SAVE*/
            /*
            Agent agent_to_save = new Agent("Pop Dana", "12345");
            agentRepository.Save(agent_to_save);*/
            
            /*FIND_ALL*/
            IEnumerable<Agent> agents = agentRepository.FindAll();
            foreach (var a in agents)
            {
                Console.WriteLine($"Agent: {a.Id}, {a.Name}, {a.Password}");
            }
 
        }
         private static void TestFlightRepo(IFlightRepository flightRepository)
        {
            // FIND ONE
            long flightId = 1;
            Flight flight = flightRepository.FindOne(flightId);
            
            if (flight != null)
            {
                Console.WriteLine($"Flight found: {flight.Destination}, {flight.DepartureDateTime}, {flight.Airport}, {flight.AvailableSeats}");
            }
            else
            {
                Console.WriteLine("Flight not found.");
            }
            
            // SAVE
            /*
            Flight flightToSave = new Flight("New York", DateTime.Now, "JFK", 100);
            flightRepository.Save(flightToSave);
            */
            
            // FIND ALL
            IEnumerable<Flight> flights = flightRepository.FindAll();
            foreach (var f in flights)
            {
                Console.WriteLine($"Flight: {f.Id}, {f.Destination}, {f.DepartureDateTime}, {f.Airport}, {f.AvailableSeats}");
            }
        }

        private static void TestBookingRepo(IBookingRepository bookingRepository)
        {
            // FIND ONE
            long bookingId = 1;
            Booking booking = bookingRepository.FindOne(bookingId);
            
            if (booking != null)
            {
                Console.WriteLine($"Booking found: Flight to {booking.Flight.Destination}, {booking.Passengers.Count} passengers, {booking.NumberOfSeats} seats");
            }
            else
            {
                Console.WriteLine("Booking not found.");
            }
            
            // SAVE
            /*
            Flight flight = flightRepository.FindOne(1); // Assuming flight with id 1 exists
            List<string> passengers = new List<string> { "John Doe", "Jane Smith" };
            Booking bookingToSave = new Booking(flight, passengers, 2);
            bookingRepository.Save(bookingToSave);
            */
            
            // FIND ALL
            IEnumerable<Booking> bookings = bookingRepository.FindAll();
            foreach (var b in bookings)
            {
                Console.WriteLine($"Booking: {b.Id}, Flight to {b.Flight.Destination}, {b.Passengers.Count} passengers, {b.NumberOfSeats} seats");
            }
        }
    }
}

