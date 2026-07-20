using Lylink.Shared.Api.Client.KeycloakTokenHandler;

namespace Lylink.Management.Api.Client;

public record class ManagementApiClientOptions : OAuth2AuthenticationOptions
{
    public string? Endpoint { get; set; }
}
