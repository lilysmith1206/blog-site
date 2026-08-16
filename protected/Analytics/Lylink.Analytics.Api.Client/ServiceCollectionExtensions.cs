using Lylink.Shared.Api.Client.Base;
using Lylink.Shared.Api.Client.HealthChecks;
using Lylink.Shared.Api.Client.KeycloakTokenHandler;
using Lylink.Shared.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lylink.Analytics.Api.Client;

public static class ServiceCollectionExtensions
{
    extension(ClientConnectivityHealthCheck<IAnalyticsApiClient>)
    {
        public static string NameKey => "analytics-api-connectivity";
    }

    public static IServiceCollection RegisterAnalyticsApiClient(
        this IServiceCollection services,
        AnalyticsApiClientOptions options
    )
    {
        if (options.Endpoint is null)
            throw new InvalidOperationException("The endpoint for the analytics API must not be null.");

        services.RegisterOAuth2HandlerDependencies();
        services.AddOptions<OAuth2AuthenticationOptions>(nameof(AnalyticsApiClient))
            .Configure(opt =>
            {
                opt.ClientId = options.ClientId;
                opt.ClientSecret = options.ClientSecret;
                opt.Authority = options.Authority;
            });

        services.AddHttpClient(nameof(AnalyticsApiClient), client =>
        {
            client.BaseAddress = new Uri(options.Endpoint);
        }).RegisterOAuth2Handler(nameof(AnalyticsApiClient));

        services.AddHttpClient();

        services.AddHealthChecks()
            .AddTimeLimitedHealthCheck<ClientConnectivityHealthCheck<IAnalyticsApiClient>>(
                ClientConnectivityHealthCheck<IAnalyticsApiClient>.NameKey
            ).AddTimeLimitedTypeActivatedHealthCheck<TokenIssuerConnectivityHealthCheck>(TokenIssuerConnectivityHealthCheck.NameKey, [options]);

        return services.AddScoped<IAnalyticsApiClient, AnalyticsApiClient>();
    }
}
