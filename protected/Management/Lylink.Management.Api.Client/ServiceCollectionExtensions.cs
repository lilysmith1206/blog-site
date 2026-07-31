using Lylink.Shared.Api.Client.HealthChecks;
using Lylink.Shared.Api.Client.KeycloakTokenHandler;
using Lylink.Shared.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lylink.Management.Api.Client;

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

        services.AddHttpClient();

        services.AddHealthChecks()
            .AddTimeLimitedHealthCheck<ClientConnectivityHealthCheck<IManagementApiClient>>("management-api-connectivity")
            .AddTimeLimitedTypeActivatedHealthCheck<TokenIssuerConnectivityHealthCheck>("management-api-token-issuer-connectivity", [options]);

        return services.AddScoped<IManagementApiClient, ManagementApiClient>();
    }
}
