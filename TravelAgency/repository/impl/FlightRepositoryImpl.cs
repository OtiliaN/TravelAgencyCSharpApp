using System.Data;
using AgentieTurismCSharp.domain;
using log4net;
using TravelAgency.exception;

namespace TravelAgency.repository.impl;

public class FlightRepositoryImpl: IFlightRepository
{
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    IDictionary<String, string> props;
    
    public FlightRepositoryImpl(IDictionary<string, string> props)
    {
        logger.Info("Creating FlightRepositoryImpl");
        this.props = props;
    }

    public Flight FindOne(long Id)
    {
        logger.InfoFormat("Finding Flight with id " + Id);
        IDbConnection connection = DBUtils.getConnection(props);
        if(Id==null)
        {
            throw new RepositoryException("Cannot find an Flight if id is null!");
        }
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = "SELECT * FROM Flights WHERE id = @id";
            IDataParameter idParam = command.CreateParameter();
            idParam.ParameterName = "@id";
            idParam.Value = Id;
            command.Parameters.Add(idParam);
            using (IDataReader result = command.ExecuteReader())
            {
                if (result.Read())
                {
                    string destination = result.GetString(1);
                    DateTime departure_date_time = result.GetDateTime(2);
                    string airport = result.GetString(3);
                    int available_seats = result.GetInt32(4);
                    Flight flight = new Flight(destination, departure_date_time, airport, available_seats);
                    flight.Id = Id;
                    logger.InfoFormat("Exiting FineOne with value {0}", flight);
                    return flight;
                }
            }
        }
        return null;
    }

    public IEnumerable<Flight> FindAll()
    {
        logger.Info("Finding all Flights");
        IDbConnection connection = DBUtils.getConnection(props);
        IList<Flight> flights = new List<Flight>();
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = "SELECT * FROM Flights";
            using (IDataReader result = command.ExecuteReader())
            {
                while (result.Read())
                {
                    long id = result.GetInt64(0);
                    string destination = result.GetString(1);
                    DateTime departure_date_time = result.GetDateTime(2);
                    string airport = result.GetString(3);
                    int available_seats = result.GetInt32(4);
                    Flight flight = new Flight(destination, departure_date_time, airport, available_seats);
                    flight.Id = id;
                    flights.Add(flight);
                }
            }
        }
        logger.InfoFormat("Exiting Find All!");
        return flights;
    }

    public Flight Save(Flight entity)
    {
        logger.InfoFormat("Saving Flight " + entity);
        IDbConnection connection = DBUtils.getConnection(props);
        if(entity==null)
        {
            String m="Cannot save flights if entity is null!\n";
            logger.InfoFormat("Sent error from repo {0}",m);
            throw new RepositoryException(m);

        }
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Flights (destination, departure_date_time, airport, available_seats) VALUES (@destination, @departure_date_time, @airport, @available_seats)";
            
            IDataParameter destinationParam = command.CreateParameter();
            destinationParam.ParameterName = "@destination";
            destinationParam.Value = entity.Destination;
            command.Parameters.Add(destinationParam);
            
            IDataParameter departureDateTimeParam = command.CreateParameter();
            departureDateTimeParam.ParameterName = "@departure_date_time";
            departureDateTimeParam.Value = entity.DepartureDateTime;
            command.Parameters.Add(departureDateTimeParam);
            
            IDataParameter airportParam = command.CreateParameter();
            airportParam.ParameterName = "@airport";
            airportParam.Value = entity.Airport;
            command.Parameters.Add(airportParam);
            
            IDataParameter availableSeatsParam = command.CreateParameter();
            availableSeatsParam.ParameterName = "@available_seats";
            availableSeatsParam.Value = entity.AvailableSeats;
            command.Parameters.Add(availableSeatsParam);
            
            command.ExecuteNonQuery();
        }
        return entity;
    }
}