using LylinkBackend.SharedClientCode.KeycloakTokenHandler;
using Microsoft.Extensions.DependencyInjection;

namespace LylinkBackend.ManagementApiClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterManagementApiClient(this IServiceCollection services, ManagementApiClientOptions options)
    {
        if (options.Endpoint is null)
            throw new InvalidOperationException("The endpoint for the management API must not be null.");

        services.RegisterOAuth2HandlerDependencies();
        services.AddOptions<OAuth2AuthenticationOptions>(nameof(ManagementApiClient))
            .Configure(opt =>
            {
                opt.ClientId = options.ClientId;
                opt.ClientSecret = options.ClientSecret;
                opt.Authority = options.Authority;
            });

        services.AddHttpClient(nameof(ManagementApiClient), client =>
        {
            client.BaseAddress = new Uri(options.Endpoint);
        }).RegisterOAuth2Handler(nameof(ManagementApiClient));

        return services.AddScoped<IManagementApiClient, ManagementApiClient>();
    }
}
