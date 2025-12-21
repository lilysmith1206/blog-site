using LylinkBackend.SharedClientCode.KeycloakTokenHandler;

namespace LylinkBackend.ManagementApiClient;

public record class ManagementApiClientOptions : OAuth2AuthenticationOptions
{
    public string? Endpoint { get; set; }
}
