using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using System.Data;

namespace Favourite.Infrastructure.Health
{
    public class DatabaseHealthCheck(DbConnectionFactory dbConnection) : IHealthCheck
    {
        private readonly DbConnectionFactory _dbConnection = dbConnection;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using IDbConnection connection = _dbConnection.OpenConnection();
                using var command = new NpgsqlCommand("SELECT 1", (NpgsqlConnection)connection);
                var result = await command.ExecuteScalarAsync(cancellationToken);

                return result switch
                {
                    int and 1 => HealthCheckResult.Healthy(),
                    _ => HealthCheckResult.Unhealthy(),
                };
            }

            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(exception: ex);
            }
        }
    }
}