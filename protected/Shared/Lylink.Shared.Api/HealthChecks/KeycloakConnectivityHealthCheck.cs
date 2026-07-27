using Lylink.Shared.Api.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Shared.Api.HealthChecks;

/// <summary>
/// This validates if a configured authority is reachable at the given URL.
/// </summary>
public class InboundKeycloakConnectivityHealthCheck : IHealthCheck
{
    private readonly ILogger<InboundKeycloakConnectivityHealthCheck> _logger;
    private readonly Uri _authority;
    private readonly IHttpClientFactory _httpClientFactory;

    public InboundKeycloakConnectivityHealthCheck(
        ILogger<InboundKeycloakConnectivityHealthCheck> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory
    )
    {
        _logger = logger;

        var inboundAuthenticationOptions = configuration
            .GetSection("InboundAuthentication")
            .Get<InboundAuthenticationOptions>();

        var authority = inboundAuthenticationOptions?.Authority;

        if (authority is null)
            throw new ArgumentException("Inbound authentication must have an expected authority value.");

        _authority = MapKeycloakAuthorityToManagementUrl(authority!);
        _httpClientFactory = httpClientFactory;
    }

    private static Uri MapKeycloakAuthorityToManagementUrl(string authority)
    {
        var authorityUriBuilder = new UriBuilder(new Uri(authority))
        {
            Port = 9000,
            Path = "/"
        };

        return authorityUriBuilder.Uri;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        using var _ = _logger.BeginScope(new { Operation = "CheckKeycloakConnectivityForHealth" });

        try
        {
            _logger.LogDebug("Creating HTTP client to check if Keycloak can be reached at {authority}", _authority);

            var httpClient = _httpClientFactory.CreateClient();

            httpClient.BaseAddress = _authority;

            _logger.LogTrace("Client created.");
            _logger.LogDebug("Checking if the well-known realm information address is reachable from the authority URL.");

            var response = await httpClient.GetAsync("health", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogTrace("The authority's well-known configuration endpoint is reachable. Considered healthy.");

                return HealthCheckResult.Healthy(description: "The authority is reachable and healthy.");
            }
            else
            {
                _logger.LogTrace("The authority's well-known configuration endpoint returned an error. Considered unhealthy.");

                return HealthCheckResult.Unhealthy(description: "The authority is reachable but unhealthy.'");
            }
        }
        catch (HttpRequestException ex) when (ex.HttpRequestError == HttpRequestError.ConnectionError)
        {
            _logger.LogTrace("A connection could be made to the authority endpoint. Considered unhealthy.");

            return new(HealthStatus.Unhealthy, description: "The authority is unreachable.", exception: ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to determine if the authority is reachable. of the Lylink database.");
            _logger.LogDebug("Returning an unhealthy status.");

            return new(HealthStatus.Unhealthy, description: "An exception occurred while checking the Lylink database connectivity.", exception: ex);
        }
    }
}
