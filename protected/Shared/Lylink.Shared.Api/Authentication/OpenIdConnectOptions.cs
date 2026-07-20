namespace Lylink.Shared.Api.Authentication;

public record class OpenIdConnectOptions
{
    public string? ExpectedAuthority { get; init; }

    public bool? RequiresHttpsAuthority { get; init; }

    public string[] RequiredScopes { get; init; } = [];

    public string? ClientId { get; init; }

    public string? ClientSecret { get; init; }
}
