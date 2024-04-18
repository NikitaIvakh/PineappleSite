using Npgsql;
using System.Data;

namespace Coupon.Infrastructure.Health;

public class DbConnectionFactory(string connectionString)
{
    public IDbConnection OpenConnection()
    {
        var connection = new NpgsqlConnection(connectionString);
        connection.Open();

        return connection;
    }
}