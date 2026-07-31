using Lylink.Shared.Api.Client.KeycloakTokenHandler;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Shared.Api.Client.HealthChecks;

public class TokenIssuerConnectivityHealthCheck : IHealthCheck
{
    private readonly ILogger<TokenIssuerConnectivityHealthCheck> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly Uri _tokenIssuerEndpoint;

    public TokenIssuerConnectivityHealthCheck(
        ILogger<TokenIssuerConnectivityHealthCheck> logger,
        IHttpClientFactory clientFactory,
        OAuth2AuthenticationOptions options
    )
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _tokenIssuerEndpoint = MapKeycloakAuthorityToManagementUrl(options.Authority!);
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
        using var _ = _logger.BeginScope(new { Operation = "CheckTokenIssuerConnectivityForHealth" });

        try
        {
            _logger.LogDebug("Creating HTTP client to check if the token issuer can be reached at {url}", _tokenIssuerEndpoint);

            var httpClient = _clientFactory.CreateClient();

            httpClient.BaseAddress = _tokenIssuerEndpoint;

            _logger.LogTrace("Client created.");
            _logger.LogDebug("Checking if the health information is reachable.");

            var response = await httpClient.GetAsync("health", cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogTrace("Health endpoint returned a success. Considered healthy.");

                return HealthCheckResult.Healthy(description: "The token issuer is reachable and healthy.");
            }
            else
            {
                _logger.LogTrace("Health endpoint returned an error. Considered unhealthy.");

                return HealthCheckResult.Unhealthy(description: "The token issuer is reachable but unhealthy.'");
            }
        }
        catch (HttpRequestException ex) when (ex.HttpRequestError == HttpRequestError.ConnectionError)
        {
            _logger.LogTrace("A connection could be made to the token issuer endpoint. Considered unhealthy.");

            return new(HealthStatus.Unhealthy, description: "The token issuer is unreachable.", exception: ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred while trying to determine if the token issuer is reachable.");
            _logger.LogDebug("Returning an unhealthy status.");

            return new(HealthStatus.Unhealthy, description: "An exception occurred while checking the token issuer connectivity.", exception: ex);
        }
    }
}