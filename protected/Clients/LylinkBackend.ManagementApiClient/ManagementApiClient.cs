using ErrorOr;
using LylinkShared.Models;
using Microsoft.Extensions.Logging;
using Polly.Retry;
using Polly;
using System.Net.Http.Json;
using LylinkBackend.ManagementShared;

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
        var postInfoByCategory = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));

            return await client.GetFromJsonAsync<Dictionary<string, List<ReferenceById>>>("/posts/by-category", token);
        }, cancellationToken);

        if (postInfoByCategory is null)
        {
            _logger.LogError("Post info retrieved is null.");

            return Error.Unexpected(description: "The post info retrieved is null. This is unexpected.");
        }

        return postInfoByCategory;
    }

    public async Task<ErrorOr<PostInfo>> GetPostById(int id, CancellationToken cancellationToken = default)
    {
        var post = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));

            return await client.GetFromJsonAsync<PostInfo>($"/posts/{id}", token);
        }, cancellationToken);

        if (post is null)
        {
            _logger.LogError("Post info retrieved is null.");

            return Error.Unexpected(description: "The post info retrieved is null. This is unexpected.");
        }

        return post;
    }

    public async Task<ErrorOr<List<ReferenceById>>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var categories = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));

            return await client.GetFromJsonAsync<List<ReferenceById>>("/categories", token);
        }, cancellationToken);

        if (categories is null)
        {
            _logger.LogError("Category info retrieved is null.");

            return Error.Unexpected(description: "The category info retrieved is null. This is unexpected.");
        }

        return categories;
    }

    public async Task<ErrorOr<CategoryInfo>> GetCategoryById(int id, CancellationToken cancellationToken = default)
    {
        var category = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));

            return await client.GetFromJsonAsync<CategoryInfo>($"/categories/{id}", token);
        }, cancellationToken);

        if (category is null)
        {
            _logger.LogError("Category info retrieved is null.");

            return Error.Unexpected(description: "The category info retrieved is null. This is unexpected.");
        }

        return category;
    }

    public async Task<ErrorOr<int>> CreatePost(PostInfo post, CancellationToken cancellationToken = default)
    {
        var responseContent = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));
            var response = await client.PostAsJsonAsync("/posts", post, token);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(token);
        }, cancellationToken);

        if (!int.TryParse(responseContent, out int postId))
        {
            _logger.LogError("Post ID given is not a valid integer.");

            return Error.Unexpected(description: "The post id given is not valid.");
        }

        return postId;
    }

    public async Task<ErrorOr<int>> CreateCategory(CategoryInfo category, CancellationToken cancellationToken = default)
    {
        var responseContent = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));
            var response = await client.PostAsJsonAsync("/categories", category, token);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(token);
        }, cancellationToken);

        if (!int.TryParse(responseContent, out int categoryId))
        {
            _logger.LogError("Category ID given is not a valid integer.");

            return Error.Unexpected(description: "The category id given is not valid.");
        }

        return categoryId;
    }

    public async Task<ErrorOr<Success>> UpdatePost(int id, PostInfo post, CancellationToken cancellationToken = default)
    {
        var responseContent = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));
            var response = await client.PutAsJsonAsync($"/posts/{id}", post, token);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(token);
        }, cancellationToken);

        return new Success();
    }

    public async Task<ErrorOr<Success>> UpdateCategory(int id, CategoryInfo category, CancellationToken cancellationToken = default)
    {
        var responseContent = await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = _clientFactory.CreateClient(nameof(ManagementApiClient));
            var response = await client.PutAsJsonAsync($"/categories/{id}", category, token);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync(token);
        }, cancellationToken);

        return new Success();
    }
}
