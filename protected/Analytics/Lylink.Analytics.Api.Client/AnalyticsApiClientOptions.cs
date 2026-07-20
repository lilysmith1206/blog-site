using Lylink.Shared.Api.Client.KeycloakTokenHandler;

namespace Lylink.Analytics.Api.Client;

public record class AnalyticsApiClientOptions : OAuth2AuthenticationOptions
{
    public string? Endpoint { get; set; }
}
