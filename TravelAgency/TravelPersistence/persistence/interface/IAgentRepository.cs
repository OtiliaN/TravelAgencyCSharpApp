
using TravelModel.domain;

namespace TravelPersistence.persistence.interfaces;

public interface IAgentRepository : IRepository<long, Agent>
{
    Agent FindByUsername(string username); 
}