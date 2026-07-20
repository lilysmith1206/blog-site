using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Lylink.Shared.Api.Client.KeycloakTokenHandler;

public class OAuth2TokenHandler : DelegatingHandler
{
    private readonly string _cachedTokenKey;

    private readonly ILogger<OAuth2TokenHandler> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly IMemoryCache _memoryCache;
    private readonly OAuth2AuthenticationOptions _oauth2Options;

    public OAuth2TokenHandler(
        ILogger<OAuth2TokenHandler> logger,
        IHttpClientFactory clientFactory,
        IMemoryCache memoryCache,
        IOptionsMonitor<OAuth2AuthenticationOptions> oauth2Options,
        string optionName
    )
    {
        _logger = logger;
        _clientFactory = clientFactory;
        _memoryCache = memoryCache;
        _oauth2Options = oauth2Options.Get(optionName);

        _cachedTokenKey = $"{optionName}_token";
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await GetAccessTokenAsync(cancellationToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string?> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var token = _memoryCache.Get<string>(_cachedTokenKey);

        if (token is not null)
            return token;

        var client = _clientFactory.CreateClient(nameof(OAuth2TokenHandler));

        var response = await client.PostAsync(
            $"{_oauth2Options.Authority}/protocol/openid-connect/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "client_credentials",
                ["client_id"] = _oauth2Options.ClientId!,
                ["client_secret"] = _oauth2Options.ClientSecret!
            }),
            cancellationToken
        );

        if (response.IsSuccessStatusCode == false)
        {
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogError("Fetching token resulted in {status}: {description}", response.StatusCode, responseContent);

            return null;
        }

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }, cancellationToken) ?? throw new InvalidOperationException("Token response could not be deserialized.");

        // Set the expiration time of the cached token to 30 seconds less than the expires in.
        // This is to handle the token being valid on this side before sent, but invalid on the server on receipt.
        var expirationTime = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 30);

        _memoryCache.Set(_cachedTokenKey, tokenResponse.AccessToken, expirationTime);

        return tokenResponse.AccessToken;
    }
}