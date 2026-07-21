namespace Lylink.Shared.Api.Authentication;

public class InboundAuthenticationOptions
{
    public BearerTokenValidationOptions? BearerToken { get; init; }

    public OpenIdConnectOptions? OpenIdConnect { get; init; }
}
