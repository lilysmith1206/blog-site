using ErrorOr;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Analytics.Api.Client;

public class AnalyticsApiConnectivityHealthCheck : IHealthCheck
{
    private readonly ILogger<AnalyticsApiConnectivityHealthCheck> _logger;
    private readonly IAnalyticsApiClient _analyticsApiClient;

    public AnalyticsApiConnectivityHealthCheck(
        ILogger<AnalyticsApiConnectivityHealthCheck> logger,
        IAnalyticsApiClient analyticsApiClient
    )
    {
        _logger = logger;
        _analyticsApiClient = analyticsApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var _ = _logger.BeginScope(new { Operation = "CheckAnalyticsApiConnectivityForHealthCheck" });

        try
        {
            var healthResponse = await _analyticsApiClient.GetHealth(cancellationToken);

            return healthResponse.Match(
                _ => HealthCheckResult.Healthy("Analytics API is healthy."),
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => HealthCheckResult.Healthy("Analytics API is assumed healthy as it handled health endpoint as 404 Not Found."),
                    ErrorType.Failure => HealthCheckResult.Unhealthy("Analytics API is unhealthy."),
                    _ => HealthCheckResult.Unhealthy("Analytics API returned an unexpected error on getting health; considered unhealthy.")
                });
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to determine if the Analytics API is healthy.");
            _logger.LogDebug("Returning an unhealthy status.");

            return new(HealthStatus.Unhealthy, description: "An exception occurred while checking the Analytics API connectivity.", exception: ex);
        }
    }
}
