using ErrorOr;
using LylinkBackend.BlogShared.Page;
using LylinkShared.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;
using System.Net.Http.Json;

namespace LylinkBackend.BlogApiClient;

public class BlogApiClient : IBlogApiClient
{
    private readonly ILogger<BlogApiClient> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ResiliencePipeline _resiliencePipeline;

    public BlogApiClient(ILogger<BlogApiClient> logger, IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;

        var retryOptions = new RetryStrategyOptions()
        {
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = 3,
            ShouldHandle = new PredicateBuilder()
                .Handle<HttpRequestException>()
        };

        _resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .AddTimeout(TimeSpan.FromSeconds(60))
            .Build();
    }

    public async Task<ErrorOr<Page>> GetIndexPage(CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<Page>(client =>
        {
            return client.GetAsync($"/pages/index", cancellationToken);
        }, [], cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "The request succeded, but was not deserialized correctly.");

        return result.Value;
    }

    public async Task<ErrorOr<Page>> GetNotFoundPage(CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<Page>(client =>
        {
            return client.GetAsync($"/pages/by-slug/404", cancellationToken);
        }, [], cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "The request succeded, but was not deserialized correctly.");

        return result.Value;
    }

    public async Task<ErrorOr<List<PageLink>>> GetMostRecentPosts(int limit, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<List<PageLink>>(client =>
        {
            return client.GetAsync($"/pages/most-recent?limit={limit}", cancellationToken);
        }, [], cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "The request succeded, but was not deserialized correctly.");

        return result.Value;
    }

    public async Task<ErrorOr<Page>> GetPageFromSlug(string slug, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<Page>(client =>
        {
            return client.GetAsync($"/pages/by-slug/{slug}", cancellationToken);
        }, new()
        {
            { HttpStatusCode.NotFound, _ => Error.NotFound(description: "The post could not be found.") }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "The request succeded, but was not deserialized correctly.");

        return result.Value;
    }

    private async Task<ErrorOr<T?>> SendAsync<T>(
        Func<HttpClient, Task<HttpResponseMessage>> sendFunction,
        Dictionary<HttpStatusCode, Func<HttpContent, ErrorOr<T?>>> errorCodeMapping,
        CancellationToken cancellationToken = default
    )
    {
        return await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(BlogApiClient));
            var response = await sendFunction(client);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Request succeeded.");

                return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
            }

            var responseContent = await response.Content.ReadAsStringAsync();

            if (errorCodeMapping.TryGetValue(response.StatusCode, out var error))
            {
                var mappedError = error(response.Content);

                _logger.LogInformation("Message failed with expected failure state: {code}, {message}", response.StatusCode, mappedError.FirstError.Description);

                return mappedError;
            }
            else
            {
                return Error.Unexpected();
            }
        }, cancellationToken);
    }
}
