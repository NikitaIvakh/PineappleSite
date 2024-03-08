using Npgsql;
using System.Data;

namespace Favourite.Infrastructure.Health
{
    public class DbConnectionFactory(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public IDbConnection OpenConnection()
        {
            var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            return connection;
        }
    }
}