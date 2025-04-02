using AgentieTurismCSharp.domain;

namespace TravelAgency.repository;

public interface IAgentRepository : IRepository<long, Agent>
{
    Agent FindByUsername(string username); 
}