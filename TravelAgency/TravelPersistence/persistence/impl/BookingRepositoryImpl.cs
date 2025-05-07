

using System.Data;
using log4net;

using TravelModel.domain;
using TravelPersistence.exceptions;
using TravelPersistence.persistence.interfaces;

namespace TravelPersistence.persistence.impl;

public class BookingRepositoryImpl:IBookingRepository
{
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    IDictionary<String, String> props;
    
    public BookingRepositoryImpl(IDictionary<String, String> props)
    {
        logger.Info("Creating BookingRepositoryImpl");
        this.props = props;
    }

    public Booking FindOne(long Id)
        {
            logger.Info("Finding Booking with id " + Id);
            IDbConnection connection = DBUtils.getConnection(props);
            if(Id == null)
            {
                throw new RepositoryException("Cannot find a Booking if id is null!");
            }
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Bookings WHERE id = @id";
                IDataParameter idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = Id;
                command.Parameters.Add(idParam);
                using (IDataReader result = command.ExecuteReader())
                {
                    if (result.Read())
                    {
                        long flightId = result.GetInt64(1);
                        List<string> passengers = new List<string>(result.GetString(2).Split(','));
                        int numberOfSeats = result.GetInt32(3);
                        
                        FlightRepositoryImpl flightRepo = new FlightRepositoryImpl(props);
                        Flight flight = flightRepo.FindOne(flightId);
                        
                        Booking booking = new Booking(flight, passengers, numberOfSeats);
                        booking.Id = Id;
                        logger.InfoFormat("Exiting FindOne with value: ", booking);
                        return booking;
                    }
                }
            }
            return null;
        }

        public IEnumerable<Booking> FindAll()
        {
            logger.Info("Finding all Bookings");
            IDbConnection connection = DBUtils.getConnection(props);
            IList<Booking> bookings = new List<Booking>();
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Bookings";
                using (IDataReader result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        long id = result.GetInt64(0);
                        long flightId = result.GetInt64(1);
                        List<string> passengers = new List<string>(result.GetString(2).Split(','));
                        int numberOfSeats = result.GetInt32(3);
                        
                        FlightRepositoryImpl flightRepo = new FlightRepositoryImpl(props);
                        Flight flight = flightRepo.FindOne(flightId);
                        
                        Booking booking = new Booking(flight, passengers, numberOfSeats);
                        booking.Id = id;
                        bookings.Add(booking);
                    }
                }
            }
            logger.InfoFormat("Exiting FindAll with {0} bookings", bookings.Count);
            return bookings;
        }

        public Booking Save(Booking entity)
        {
            logger.InfoFormat("Saving Booking " + entity);
            IDbConnection connection = DBUtils.getConnection(props);
            if(entity == null)
            {
                string message = "Cannot save Booking if entity is null!";
                logger.InfoFormat("Sent error from repo {0}", message);
                throw new RepositoryException(message);
            }
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "INSERT INTO Bookings (id_flight, passengers, numberOfSeats) VALUES (@id_flight, @passengers, @numberOfSeats)";
                
                IDataParameter flightIdParam = command.CreateParameter();
                flightIdParam.ParameterName = "@id_flight";
                flightIdParam.Value = entity.Flight.Id;
                command.Parameters.Add(flightIdParam);
                
                IDataParameter passengersParam = command.CreateParameter();
                passengersParam.ParameterName = "@passengers";
                passengersParam.Value = string.Join(",", entity.Passengers);
                command.Parameters.Add(passengersParam);
                
                IDataParameter numberOfSeatsParam = command.CreateParameter();
                numberOfSeatsParam.ParameterName = "@numberOfSeats";
                numberOfSeatsParam.Value = entity.NumberOfSeats;
                command.Parameters.Add(numberOfSeatsParam);
                
                command.ExecuteNonQuery();
            }
            return entity;
        }
  
}