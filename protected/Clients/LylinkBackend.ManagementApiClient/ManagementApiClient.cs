using ErrorOr;
using LylinkBackend.ManagementShared;
using LylinkShared.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using System.Net;
using System.Net.Http.Json;

namespace LylinkBackend.ManagementApiClient;

public class ManagementApiClient : IManagementApiClient
{
    private readonly ILogger<ManagementApiClient> _logger;
    private readonly IHttpClientFactory _clientFactory;
    private readonly ResiliencePipeline _resiliencePipeline;

    public ManagementApiClient(ILogger<ManagementApiClient> logger, IHttpClientFactory clientFactory)
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

    private async Task<ErrorOr<T?>> SendAsync<T>(
        Func<HttpClient, Task<HttpResponseMessage>> sendFunction,
        Dictionary<HttpStatusCode, Func<HttpContent, ErrorOr<T?>>> errorCodeMapping,
        CancellationToken cancellationToken = default
    )
    {
        return await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));
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
