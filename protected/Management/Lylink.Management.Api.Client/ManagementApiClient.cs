using ErrorOr;
using Lylink.Management.Shared;
using Lylink.Shared.Api.Client.BaseClient;
using Lylink.Shared.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;
using System.Net.Http.Json;

namespace Lylink.Management.Api.Client;

public class ManagementApiClient : BaseClient, IManagementApiClient
{
    private readonly ILogger<ManagementApiClient> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public ManagementApiClient(ILogger<ManagementApiClient> logger, IHttpClientFactory clientFactory) : base(logger)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    public async Task<ErrorOr<Dictionary<string, List<ReferenceById>>>> GetPostsByCategoryAsync(CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<Dictionary<string, List<ReferenceById>>>(client =>
        {
            return client.GetAsync($"/posts/by-category", cancellationToken);
        }, [], cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "The request succeded, but was not deserialized correctly.");

        return result.Value;
    }

    public async Task<ErrorOr<PostInfo>> GetPostById(int id, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<PostInfo>(client =>
        {
            return client.GetAsync($"/posts/{id}", cancellationToken);
        }, new()
        {
            { HttpStatusCode.NotFound, _ => Error.NotFound(description: "A category was not found with the given ID.") }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "A category was found, but it was not deserialized correctly.");

        return result.Value;
    }

    public async Task<ErrorOr<List<ReferenceById>>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<List<ReferenceById>>(client =>
        {
            return client.GetAsync($"/categories", cancellationToken);
        }, new()
        {
            { HttpStatusCode.NotFound, _ => Error.NotFound(description: "A post was not found with the given ID.") }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "Categories were found, but were not deserialized correctly.");

        return result.Value;
    }

    public async Task<ErrorOr<CategoryInfo>> GetCategoryById(int id, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<CategoryInfo>(client =>
        {
            return client.GetAsync($"/categories/{id}", cancellationToken);
        }, new()
        {
            { HttpStatusCode.NotFound, _ => Error.NotFound(description: "A category was not found with the given ID.") }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        if (result.Value is null)
            return Error.Unexpected(description: "A category was found, but it was not deserialized correctly.");

        return result.Value;
    }

    public async Task<ErrorOr<int>> CreatePost(PostInfo post, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<int>(client =>
        {
            return client.PostAsJsonAsync($"/posts", post, cancellationToken);
        }, new()
        {
            { HttpStatusCode.Conflict, HandleConflictError }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        return result.Value;
    }

    public async Task<ErrorOr<int>> CreateCategory(CategoryInfo category, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<int>(client =>
        {
            return client.PostAsJsonAsync($"/categories", category, cancellationToken);
        }, new()
        {
            { HttpStatusCode.Conflict, HandleConflictError }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        return result.Value;
    }

    private static ErrorOr<int> HandleConflictError(HttpContent content)
    {
        var conflictDetailsTask = content.ReadFromJsonAsync<ConflictDetails>();
        conflictDetailsTask.Wait();
        var conflictDetails = conflictDetailsTask.Result;

        if (conflictDetails is null)
            return Error.Unexpected(description: "The conflict details could not be deserialized.");

        return Error.Conflict(description: "A page with the given slug already exists.", metadata: new() { { "conflict", conflictDetails.ConflictingId } });
    }

    public async Task<ErrorOr<Success>> UpdatePost(int id, PostInfo post, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<string>(client =>
        {
            return client.PutAsJsonAsync($"/posts/{id}", post, cancellationToken);
        }, new() {
            { HttpStatusCode.NotFound, _ => Error.NotFound(description: "The post to update was not found.") }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        return new Success();
    }

    public async Task<ErrorOr<Success>> UpdateCategory(int id, CategoryInfo category, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<string>(client =>
        {
            return client.PutAsJsonAsync($"/categories/{id}", category, cancellationToken);
        }, new() {
            { HttpStatusCode.NotFound, _ => Error.NotFound(description: "The category to update was not found.") }
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        return new Success();
    }

    public async Task<ErrorOr<Success>> GetHealth(CancellationToken cancellationToken = default)
    {
        var result = await SendAsync(client =>
        {
            return client.GetAsync($"/health", cancellationToken);
        }, new() {
            { HttpStatusCode.NotFound, _ => Error.NotFound("Health endpoint was not found." )},
            { HttpStatusCode.ServiceUnavailable, _ => Error.Failure("Service is unhealthy." )}
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        return new Success();
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
        return _clientFactory.CreateClient(nameof(ManagementApiClient));
    }
}
