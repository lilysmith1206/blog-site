using ErrorOr;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Blog.Api.Client;

public class BlogApiConnectivityHealthCheck : IHealthCheck
{
    private readonly ILogger<BlogApiConnectivityHealthCheck> _logger;
    private readonly IBlogApiClient _blogApiClient;

    public BlogApiConnectivityHealthCheck(
        ILogger<BlogApiConnectivityHealthCheck> logger,
        IBlogApiClient blogApiClient
    )
    {
        _logger = logger;
        _blogApiClient = blogApiClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var _ = _logger.BeginScope(new { Operation = "CheckBlogApiConnectivityForHealthCheck" });

        try
        {
            var healthResponse = await _blogApiClient.GetHealth(cancellationToken);

            return healthResponse.Match(
                _ => HealthCheckResult.Healthy("Blog API is healthy."),
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => HealthCheckResult.Healthy("Blog API is assumed healthy as it handled health endpoint as 404 Not Found."),
                    ErrorType.Failure => HealthCheckResult.Unhealthy("Blog API is unhealthy."),
                    _ => HealthCheckResult.Unhealthy("Blog API returned an unexpected error on getting health; considered unhealthy.")
                });
        } 
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to determine if the Blog API is healthy.");
            _logger.LogDebug("Returning an unhealthy status.");

            return new(HealthStatus.Unhealthy, description: "An exception occurred while checking the Blog API connectivity.", exception: ex);
        }
    }
}
