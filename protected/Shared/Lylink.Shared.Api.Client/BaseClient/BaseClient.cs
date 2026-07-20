using System.Net;
using System.Net.Http.Json;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Polly;

namespace Lylink.Shared.Api.Client.BaseClient;

public abstract class BaseClient
{
    private readonly ILogger _logger;
    private readonly ResiliencePipeline _resiliencePipeline;

    public BaseClient(ILogger logger)
    {
        _logger = logger;
        _resiliencePipeline = GetResiliencePipeline();
    }

    protected async Task<ErrorOr<T?>> SendAsync<T>(
        Func<HttpClient, Task<HttpResponseMessage>> sendFunction,
        Dictionary<HttpStatusCode, Func<HttpContent, ErrorOr<T?>>> errorCodeMapping,
        CancellationToken cancellationToken = default
    )
    {
        return await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = GetHttpClient();
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

    protected abstract ResiliencePipeline GetResiliencePipeline();

    protected abstract HttpClient GetHttpClient();
}
