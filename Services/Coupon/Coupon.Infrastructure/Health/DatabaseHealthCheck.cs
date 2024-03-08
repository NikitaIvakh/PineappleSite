using Npgsql;
using System.Data;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Coupon.Infrastructure.Health
{
    public sealed class DatabaseHealthCheck(DbConnectionFactory dbConnectionFactory) : IHealthCheck
    {
        private readonly DbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using IDbConnection connection = _dbConnectionFactory.OpenConnection();

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