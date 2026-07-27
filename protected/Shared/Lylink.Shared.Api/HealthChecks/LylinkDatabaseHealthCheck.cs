using Lylink.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Shared.Api.HealthChecks;

public class LylinkDatabaseHealthCheck : IHealthCheck
{
    private readonly ILogger<LylinkDatabaseHealthCheck> _logger;
    private readonly IDbContextFactory<LylinkdbContext> _contextFactory;

    public LylinkDatabaseHealthCheck(
        ILogger<LylinkDatabaseHealthCheck> logger,
        IDbContextFactory<LylinkdbContext> contextFactory
    )
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var _ = _logger.BeginScope(new { Operation = "CheckLylinkDatabaseHealth" });

        try
        {
            _logger.LogDebug("Creating context to check for connectivity.");

            var dbContext = await _contextFactory.CreateDbContextAsync();

            _logger.LogTrace("Context created.");
            _logger.LogDebug("Checking connectivity on database context.");

            if (await dbContext.Database.CanConnectAsync(cancellationToken))
            {
                _logger.LogTrace("Lylink database can be connected to.");

                return HealthCheckResult.Healthy(description: "Lylink database can be connected to.");
            }
            else
            {
                _logger.LogTrace("Lylink database cannot be connected to.");

                return HealthCheckResult.Unhealthy(description: "Lylink database cannot be connected to.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to determine the connectivity of the Lylink database.");
            _logger.LogDebug("Returning an unhealthy status.");

            return new(HealthStatus.Unhealthy, description: "An exception occurred while checking the Lylink database connectivity.", exception: ex);
        }
    }
}
