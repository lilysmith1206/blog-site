using System.Text.Json.Serialization;

namespace Lylink.Shared.Api.Authentication;

public class InboundAuthenticationOptions
{
    [JsonIgnore]
    public string? Authority { get => BearerToken?.ExpectedAuthority ?? OpenIdConnect?.ExpectedAuthority; }

    public BearerTokenValidationOptions? BearerToken { get; init; }

    public OpenIdConnectOptions? OpenIdConnect { get; init; }
}
