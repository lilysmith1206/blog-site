using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Lylink.Shared.Api.Client.KeycloakTokenHandler;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterOAuth2HandlerDependencies(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(OAuth2TokenHandler));
        services.AddMemoryCache();

        return services;
    }

    public static IHttpClientBuilder RegisterOAuth2Handler(this IHttpClientBuilder builder, string name)
    {
        return builder.AddHttpMessageHandler(serviceProvider =>
        {
            return new OAuth2TokenHandler(
                serviceProvider.GetRequiredService<ILogger<OAuth2TokenHandler>>(),
                serviceProvider.GetRequiredService<IHttpClientFactory>(),
                serviceProvider.GetRequiredService<IMemoryCache>(),
                serviceProvider.GetRequiredService<IOptionsMonitor<OAuth2AuthenticationOptions>>(),
                name
            );
        });
    }
}
