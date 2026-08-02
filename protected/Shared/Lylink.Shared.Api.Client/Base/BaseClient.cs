using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ErrorOr;
using Lylink.Shared.Models;
using Microsoft.Extensions.Logging;
using Polly;

namespace Lylink.Shared.Api.Client.Base;

public abstract class BaseClient : IHealthClient
{
    private readonly ILogger _logger;
    private readonly ResiliencePipeline _resiliencePipeline;

    public BaseClient(ILogger logger)
    {
        _logger = logger;
        _resiliencePipeline = GetResiliencePipeline();
    }

    public async Task<ErrorOr<Success>> GetHealth(CancellationToken cancellationToken = default)
    {
        var result = await SendAsync(client =>
        {
            return client.GetAsync($"/health", cancellationToken);
        }, new() {
            { HttpStatusCode.NotFound, _ => Error.NotFound("Health endpoint was not found." )},
            { HttpStatusCode.ServiceUnavailable, _ => Error.NotAvailable([])}
        }, cancellationToken);

        if (result.IsError)
            return result.FirstError;

        return new Success();
    }

    protected async Task<ErrorOr<T?>> SendAsync<T>(
        Func<HttpClient, Task<HttpResponseMessage>> sendFunction,
        Dictionary<HttpStatusCode, Func<string, ErrorOr<T?>>> errorCodeMapping,
        CancellationToken cancellationToken = default
    )
    {
        if (!errorCodeMapping.ContainsKey(HttpStatusCode.ServiceUnavailable))
            errorCodeMapping.Add(HttpStatusCode.ServiceUnavailable, HandleServiceUnavailable<T>);

        return await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = GetHttpClient();
            var response = await sendFunction(client);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Request succeeded.");

                return await response.Content.ReadFromJsonAsync<T>(cancellationToken);
            }

            if (errorCodeMapping.TryGetValue(response.StatusCode, out var error))
            {
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
                var mappedError = error(responseString);

                _logger.LogInformation("Message failed with expected failure state: {code}, {message}", response.StatusCode, mappedError.FirstError.Description);

                return mappedError;
            }
            else
            {
                return Error.Unexpected();
            }
        }, cancellationToken);
    }

    protected async Task<ErrorOr<Success>> SendAsync(
        Func<HttpClient, Task<HttpResponseMessage>> sendFunction,
        Dictionary<HttpStatusCode, Func<string, ErrorOr<Success>>> errorCodeMapping,
        CancellationToken cancellationToken = default
    )
    {
        if (!errorCodeMapping.ContainsKey(HttpStatusCode.ServiceUnavailable))
            errorCodeMapping.Add(HttpStatusCode.ServiceUnavailable, HandleServiceUnavailable<Success>);

        return await _resiliencePipeline.ExecuteAsync(async token =>
        {
            var client = GetHttpClient();
            var response = await sendFunction(client);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Request succeeded.");

                return new Success();
            }

            if (errorCodeMapping.TryGetValue(response.StatusCode, out var error))
            {
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
                var mappedError = error(responseString);

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

    private static ErrorOr<T?> HandleServiceUnavailable<T>(string content)
    {
        var repsonse = JsonSerializer.Deserialize<ServiceUnavailableResponse>(content);

        return Error.NotAvailable(repsonse?.FailedChecks ?? []);
    }
}
