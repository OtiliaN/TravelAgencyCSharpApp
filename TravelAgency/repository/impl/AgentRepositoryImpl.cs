using System.Data;
using AgentieTurismCSharp.domain;
using log4net;
using TravelAgency.exception;

namespace TravelAgency.repository.impl;

public class AgentRepositoryImpl: IAgentRepository
{
    private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    IDictionary<String, String> props;
    
    public AgentRepositoryImpl(IDictionary<String, String> props)
    {
        logger.Info("Creating AgentRepositoryImpl");
        this.props = props;
    } 
    
    public Agent FindOne(long Id)
    {
        logger.Info("Finding Agent with id " + Id);
        IDbConnection connection = DBUtils.getConnection(props);
        if(Id==null)
        {
            throw new RepositoryException("Cannot find an Agent if id is null!");
        }
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = "SELECT * FROM Agents WHERE id = @id";
            IDataParameter idParam = command.CreateParameter();
            idParam.ParameterName = "@id";
            idParam.Value = Id;
            command.Parameters.Add(idParam);
            using (IDataReader result = command.ExecuteReader())
            {
                if (result.Read())
                {
                    string name = result.GetString(1);
                    string password = result.GetString(2);
                    Agent agent = new Agent(name, password);
                    agent.Id = Id;
                    logger.InfoFormat("Exiting FineOne with value:", agent);
                    return agent;
                }
            }
        }
        return null;
        
    }

    public IEnumerable<Agent> FindAll()
    {
        logger.Info("Finding all Agents");
        IDbConnection connection = DBUtils.getConnection(props);
        IList<Agent> agents = new List<Agent>();
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = "SELECT * FROM Agents";
            using (IDataReader result = command.ExecuteReader())
            {
                while (result.Read())
                {
                    long id = result.GetInt64(0);
                    string name = result.GetString(1);
                    string password = result.GetString(2);
                    Agent agent = new Agent(name, password);
                    agent.Id = id;
                    agents.Add(agent);
                }
            }
        }
        logger.InfoFormat("Exiting Find All!");
        return agents;
    }

    public Agent Save(Agent entity)
    {
        logger.Info("Saving Agent " + entity);
        IDbConnection connection = DBUtils.getConnection(props);
        if(entity==null)
        {
            String m="Cannot save agents if entity is null!\n";
            logger.InfoFormat("Sent error from repo {0}",m);
            throw new RepositoryException(m);

        }
        using (IDbCommand command = connection.CreateCommand())
        {
            command.CommandText = "INSERT INTO Agents (name, password) VALUES (@name, @password)";
            
            IDataParameter nameParam = command.CreateParameter();
            nameParam.ParameterName = "@name";
            nameParam.Value = entity.Name;
            command.Parameters.Add(nameParam);
            
            IDataParameter passwordParam = command.CreateParameter();
            passwordParam.ParameterName = "@password";
            passwordParam.Value = entity.Password;
            
            command.Parameters.Add(passwordParam);
            command.ExecuteNonQuery();
        }
        logger.InfoFormat("Agency saved with value: " + entity);
        return entity;
    }
}