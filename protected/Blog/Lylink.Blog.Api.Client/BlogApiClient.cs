using ErrorOr;
using Lylink.Blog.Shared.Page;
using Lylink.Shared.Api.Client.Base;
using Lylink.Shared.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;

namespace Lylink.Blog.Api.Client;

public class BlogApiClient : BaseClient, IBlogApiClient
{
    private readonly ILogger<BlogApiClient> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public BlogApiClient(ILogger<BlogApiClient> logger, IHttpClientFactory clientFactory) : base(logger)
    {
        _logger = logger;
        _clientFactory = clientFactory;
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
    
    protected override ResiliencePipeline GetResiliencePipeline()
    {
        var retryOptions = new RetryStrategyOptions()
        {
            Delay = TimeSpan.FromSeconds(1),
            MaxRetryAttempts = 3,
            ShouldHandle = new PredicateBuilder()
                .Handle<HttpRequestException>()
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .AddTimeout(TimeSpan.FromSeconds(60))
            .Build();
    }

    protected override HttpClient GetHttpClient()
    {
        return _clientFactory.CreateClient(nameof(BlogApiClient));
    }
}
