using System;
using System.Collections.Generic;
using System.Data;
using AgentieTurismCSharp.domain;
using log4net;
using TravelAgency.exception;

namespace TravelAgency.repository.impl
{
    public class FlightRepositoryImpl : IFlightRepository
    {
        private static readonly ILog logger =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IDictionary<string, string> props;
        private const string DateTimeFormat = "yyyy-MM-dd HH:mm";
        private const string DateFormat = "yyyy-MM-dd";

        public FlightRepositoryImpl(IDictionary<string, string> props)
        {
            logger.Info("Creating FlightRepositoryImpl");
            this.props = props;
        }

        public Flight FindOne(long id)
        {
            logger.InfoFormat("Finding Flight with id {0}", id);
            IDbConnection connection = DBUtils.getConnection(props);
            if (id == 0)
            {
                throw new RepositoryException("Cannot find a Flight if id is 0!");
            }

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "SELECT * FROM Flights WHERE id = @id";
                IDataParameter idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = id;
                command.Parameters.Add(idParam);
                using (IDataReader result = command.ExecuteReader())
                {
                    if (result.Read())
                    {
                        string destination = result.GetString(1);
                        DateTime departureDateTime = DateTime.ParseExact(result.GetString(2), DateTimeFormat, null);
                        string airport = result.GetString(3);
                        int availableSeats = result.GetInt32(4);
                        Flight flight = new Flight(destination, departureDateTime, airport, availableSeats);
                        flight.Id = id;
                        logger.InfoFormat("Exiting FindOne with value {0}", flight);
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
                        DateTime departureDateTime = DateTime.ParseExact(result.GetString(2), DateTimeFormat, null);
                        string airport = result.GetString(3);
                        int availableSeats = result.GetInt32(4);
                        Flight flight = new Flight(destination, departureDateTime, airport, availableSeats);
                        flight.Id = id;
                        flights.Add(flight);
                    }
                }
            }

            logger.InfoFormat("Exiting FindAll!");
            return flights;
        }

        public Flight Save(Flight entity)
        {
            logger.InfoFormat("Saving Flight {0}", entity);
            IDbConnection connection = DBUtils.getConnection(props);
            if (entity == null)
            {
                string message = "Cannot save flights if entity is null!\n";
                logger.InfoFormat("Sent error from repo {0}", message);
                throw new RepositoryException(message);
            }

            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    "INSERT INTO Flights (destination, departure_date_time, airport, available_seats) VALUES (@destination, @departure_date_time, @airport, @available_seats)";

                IDataParameter destinationParam = command.CreateParameter();
                destinationParam.ParameterName = "@destination";
                destinationParam.Value = entity.Destination;
                command.Parameters.Add(destinationParam);

                IDataParameter departureDateTimeParam = command.CreateParameter();
                departureDateTimeParam.ParameterName = "@departure_date_time";
                departureDateTimeParam.Value =
                    entity.DepartureDateTime.ToString(DateTimeFormat); // Storing DateTime as formatted TEXT
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

        public IEnumerable<Flight> FindByDestinationAndDeparture(string destination, DateTime departureDate)
        {
            logger.InfoFormat("Finding Flights with destination {0} and departure date {1}", destination,
                departureDate);
            IDbConnection connection = DBUtils.getConnection(props);
            IList<Flight> flights = new List<Flight>();
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText =
                    "SELECT * FROM Flights WHERE destination = @destination AND strftime('%Y-%m-%d', departure_date_time) = @departure_date";

                IDataParameter destinationParam = command.CreateParameter();
                destinationParam.ParameterName = "@destination";
                destinationParam.Value = destination;
                command.Parameters.Add(destinationParam);

                IDataParameter departureDateParam = command.CreateParameter();
                departureDateParam.ParameterName = "@departure_date";
                departureDateParam.Value = departureDate.ToString(DateFormat); // Format standard pentru SQLite
                command.Parameters.Add(departureDateParam);

                using (IDataReader result = command.ExecuteReader())
                {
                    while (result.Read())
                    {
                        long id = result.GetInt64(0);
                        string dest = result.GetString(1);
                        DateTime depDateTime = DateTime.ParseExact(result.GetString(2), DateTimeFormat, null);
                        string airport = result.GetString(3);
                        int availableSeats = result.GetInt32(4);
                        Flight flight = new Flight(dest, depDateTime, airport, availableSeats);
                        flight.Id = id;
                        flights.Add(flight);
                    }
                }
            }

            logger.InfoFormat("Exiting FindByDestinationAndDeparture!");
            return flights;
        }
        public void Update(Flight entity)
        {
            logger.InfoFormat("Updating Flight {0}", entity);
            IDbConnection connection = DBUtils.getConnection(props);
            if (entity == null)
            {
                string message = "Cannot update flights if entity is null!\n";
                logger.InfoFormat("Sent error from repo {0}", message);
                throw new RepositoryException(message);
            }
            using (IDbCommand command = connection.CreateCommand())
            {
                command.CommandText = "UPDATE Flights SET destination = @destination, departure_date_time = @departure_date_time, airport = @airport, available_seats = @available_seats WHERE id = @id";

                IDataParameter idParam = command.CreateParameter();
                idParam.ParameterName = "@id";
                idParam.Value = entity.Id;
                command.Parameters.Add(idParam);

                IDataParameter destinationParam = command.CreateParameter();
                destinationParam.ParameterName = "@destination";
                destinationParam.Value = entity.Destination;
                command.Parameters.Add(destinationParam);

                IDataParameter departureDateTimeParam = command.CreateParameter();
                departureDateTimeParam.ParameterName = "@departure_date_time";
                departureDateTimeParam.Value = entity.DepartureDateTime.ToString(DateTimeFormat); // Storing DateTime as formatted TEXT
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
        }


    }
}