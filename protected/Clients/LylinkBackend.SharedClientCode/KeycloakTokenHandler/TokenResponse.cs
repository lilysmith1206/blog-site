namespace LylinkBackend.SharedClientCode.KeycloakTokenHandler;

internal class TokenResponse
{
    public string? AccessToken { get; set; }

    public int ExpiresIn { get; set; }
}
