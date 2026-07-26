namespace Lylink.Management.Api.Client;

internal class TokenResponse
{
    public string? AccessToken { get; set; }
 
    public int ExpiresIn { get; set; }
}
