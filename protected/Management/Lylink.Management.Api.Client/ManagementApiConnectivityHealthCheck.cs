using ErrorOr;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Management.Api.Client;

public class ManagementApiConnectivityHealthCheck : IHealthCheck
{
    private readonly ILogger<ManagementApiConnectivityHealthCheck> _logger;
    private readonly IManagementApiClient _managementApiClient;

    public ManagementApiConnectivityHealthCheck(
        ILogger<ManagementApiConnectivityHealthCheck> logger,
        IManagementApiClient managementApiClient
    )
    {
        _logger = logger;
        _managementApiClient = managementApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var _ = _logger.BeginScope(new { Operation = "CheckManagementApiConnectivityForHealthCheck" });

        try
        {
            var healthResponse = await _managementApiClient.GetHealth(cancellationToken);

            return healthResponse.Match(
                _ => HealthCheckResult.Healthy("Management API is healthy."),
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => HealthCheckResult.Healthy("Management API is assumed healthy as it handled health endpoint as 404 Not Found."),
                    ErrorType.Failure => HealthCheckResult.Unhealthy("Management API is unhealthy."),
                    _ => HealthCheckResult.Unhealthy("Management API returned an unexpected error on getting health; considered unhealthy.")
                });
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to determine if the Management API is healthy.");
            _logger.LogDebug("Returning an unhealthy status.");

            return new(HealthStatus.Unhealthy, description: "An exception occurred while checking the Management API health status.", exception: ex);
        }
    }
}
