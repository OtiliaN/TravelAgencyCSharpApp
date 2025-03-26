using System.Data;
using System.Reflection;

namespace TravelAgency.connection;

public abstract class ConnectionFactory
{
    protected ConnectionFactory()
    {
        
    }
    
    private static ConnectionFactory instance;

    public static ConnectionFactory getInstance()
    {
        if (instance == null)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Type[] types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (type.IsSubclassOf(typeof(ConnectionFactory)))
                {
                    instance = (ConnectionFactory)Activator.CreateInstance(type);
                }
            }
        }
        return instance;
    }
    
    public abstract IDbConnection createConnection(IDictionary<string, string> properties);
}