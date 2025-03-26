using AgentieTurismCSharp.domain;

namespace TravelAgency.repository;

public interface IRepository<ID, E> where E : Entity<ID>
{
    /*
     * Find the entity with the given id
     * @param id
     */
    E FindOne(ID Id);
    
    /*
     * @return all entities
     */
    IEnumerable<E> FindAll();
    
    /*
     * Save an entity in the repository
     * @param entity
     */ 
    E Save(E entity);
}