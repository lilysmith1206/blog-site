using LylinkBackend.SharedClientCode.KeycloakTokenHandler;

namespace LylinkBackend.Analytics.Client;

public record class AnalyticsApiClientOptions : OAuth2AuthenticationOptions
{
    public string? Endpoint { get; set; }
}
