namespace LylinkBackend.ManagementApiClient;

internal class TokenResponse
{
    public string? AccessToken { get; set; }
 
    public int ExpiresIn { get; set; }
}
