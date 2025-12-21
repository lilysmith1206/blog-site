namespace LylinkBackend.SharedApiCode.Authentication;

public class InboundAuthenticationOptions
{
    public BearerTokenValidationOptions? BearerTokenValidation { get; init; }
}
