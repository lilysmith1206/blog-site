using ErrorOr;
using Microsoft.Extensions.Logging;
using Lylink.Shared.Api.Client.BaseClient;
using Lylink.Analytics.Models;
using Polly;
using Polly.Retry;
using System.Net.Http.Json;

namespace Lylink.Analytics.Api.Client;

public class AnalyticsApiClient : BaseClient, IAnalyticsApiClient
{
    private readonly ILogger<AnalyticsApiClient> _logger;
    private readonly IHttpClientFactory _clientFactory;

    public AnalyticsApiClient(ILogger<AnalyticsApiClient> logger, IHttpClientFactory clientFactory) : base(logger)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    public async Task<ErrorOr<int>> CreateSuccessVisitAnalytic(NewSuccessVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<int>(client =>
        {
            return client.PostAsJsonAsync($"/analytics/visit/success", visitorAnalytic, cancellationToken);
        }, [], cancellationToken);

        if (result.IsError)
            return result.FirstError;

        return result.Value;
    }

    public async Task<ErrorOr<int>> CreateFailureVisitAnalytic(NewFailureVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default)
    {
        var result = await SendAsync<int>(client =>
        {
            return client.PostAsJsonAsync($"/analytics/visit/failure", visitorAnalytic, cancellationToken);
        }, [], cancellationToken);

        if (result.IsError)
            return result.FirstError;

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
        return _clientFactory.CreateClient(nameof(AnalyticsApiClient));
    }
}
