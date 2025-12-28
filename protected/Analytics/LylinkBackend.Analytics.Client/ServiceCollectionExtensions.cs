using LylinkBackend.SharedClientCode.KeycloakTokenHandler;
using Microsoft.Extensions.DependencyInjection;

namespace LylinkBackend.Analytics.Client;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterAnalyticsApiClient(this IServiceCollection services, AnalyticsApiClientOptions options)
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

        return services.AddScoped<IAnalyticsApiClient, AnalyticsApiClient>();
    }
}
