using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LylinkBackend.SharedClientCode.KeycloakTokenHandler;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterKeycloakTokenHandlerDependencies(this IServiceCollection services)
    {
        services.AddHttpClient(nameof(OAuth2TokenHandler));

        return services.AddTransient<OAuth2TokenHandler>();
    }

    public static IHttpClientBuilder RegisterKeycloakHandler(this IHttpClientBuilder builder, string name)
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
