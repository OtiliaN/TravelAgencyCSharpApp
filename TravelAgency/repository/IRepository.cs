using AgentieTurismCSharp.domain;

namespace TravelAgency.repository;

public interface IRepository<ID, E> where E : Entity<ID>
{
    E FindOne(ID Id);
    IEnumerable<E> FindAll();
    E Save(E entity);
}