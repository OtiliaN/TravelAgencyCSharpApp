using System.Data;

    
namespace TravelAgency.repository.impl;

public class DBUtils
{
    private static IDbConnection instance = null;


    public static IDbConnection getConnection(IDictionary<string,string> props)
    {
        if (instance == null || instance.State == System.Data.ConnectionState.Closed)
        {
            instance = getNewConnection(props);
            instance.Open();
        }
        return instance;
    }

    private static IDbConnection getNewConnection(IDictionary<string,string> props)
    {
			
        return connection.ConnectionFactory.getInstance().createConnection(props);


    }
}