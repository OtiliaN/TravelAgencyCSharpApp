using System.Data;
using System.Data.SQLite;

namespace TravelPersistence.connection;

public class SqlLiteConnectionFactory : ConnectionFactory
{
    public override IDbConnection createConnection(IDictionary<string, string> properties)
    {
        string connectionString = properties["connectionString"];
        Console.WriteLine($"Connection string: {connectionString}");
        return new SQLiteConnection(connectionString);
    }
}