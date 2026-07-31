using ErrorOr;
using Lylink.Shared.Api.Client.Base;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Shared.Api.Client.HealthChecks;

public class ClientConnectivityHealthCheck<TClient> : IHealthCheck where TClient : IHealthClient
{
    private readonly ILogger<ClientConnectivityHealthCheck<TClient>> _logger;
    private readonly TClient _healthClient;

    public ClientConnectivityHealthCheck(
        ILogger<ClientConnectivityHealthCheck<TClient>> _logger,
        TClient healthClient
    )
    {
        this._logger = _logger;
        _healthClient = healthClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var _ = _logger.BeginScope(new { Operation = "CheckTargetApiConnectivityForHealthCheck", Target = typeof(TClient).Name });

        try
        {
            var healthResponse = await _healthClient.GetHealth(cancellationToken);

            return healthResponse.Match(
                _ => HealthCheckResult.Healthy("Target API is healthy."),
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => HealthCheckResult.Healthy("Target API is assumed healthy as it handled health endpoint as 404 Not Found."),
                    ErrorType.Failure => HealthCheckResult.Unhealthy("Target API is unhealthy."),
                    _ => HealthCheckResult.Unhealthy("Target API returned an unexpected error on getting health; considered unhealthy.")
                });
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to determine if the Target API is healthy.");
            _logger.LogDebug("Returning an unhealthy status.");

            return new(HealthStatus.Unhealthy, description: "An exception occurred while checking the Target API health status.", exception: ex);
        }
    }
}