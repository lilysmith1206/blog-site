namespace Lylink.Shared.Api.Authentication;

public class InboundAuthenticationOptions
{
    public BearerTokenValidationOptions? BearerTokenValidation { get; init; }

    public OpenIdConnectOptions? OpenIdConnect { get; init; }
}
