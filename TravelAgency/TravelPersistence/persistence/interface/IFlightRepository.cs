
using TravelModel.domain;

namespace TravelPersistence.persistence.interfaces;


public interface IFlightRepository : IRepository<long, Flight>
{
    public IEnumerable<Flight> FindByDestinationAndDeparture(string destination, DateTime departureDateTime);
    public void Update(Flight flight);

}