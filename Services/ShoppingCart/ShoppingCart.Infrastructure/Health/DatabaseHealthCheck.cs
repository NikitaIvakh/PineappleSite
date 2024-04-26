using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace ShoppingCart.Infrastructure.Health;

public sealed class DatabaseHealthCheck(DbConnectionFactory connectionFactory) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            using var dbConnection = connectionFactory.OpenConnection();
            await using var command = new NpgsqlCommand("SELECT 1", (NpgsqlConnection)dbConnection);
            var result = await command.ExecuteScalarAsync(cancellationToken);

            return result switch
            {
                1 => HealthCheckResult.Healthy(),
                _ => HealthCheckResult.Unhealthy(),
            };
        }

        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}