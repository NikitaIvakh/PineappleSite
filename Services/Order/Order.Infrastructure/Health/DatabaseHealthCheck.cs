using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;
using System.Data;

namespace Order.Infrastructure.Health
{
    public class DatabaseHealthCheck(DbConnectionFactory connectionFactory) : IHealthCheck
    {
        private readonly DbConnectionFactory _connectionFactory = connectionFactory;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using IDbConnection connection = _connectionFactory.OpenConnection();
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