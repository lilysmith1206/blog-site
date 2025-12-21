namespace LylinkBackend.SharedClientCode.KeycloakTokenHandler;

public record class OAuth2AuthenticationOptions
{
    public string? Authority { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }
}
