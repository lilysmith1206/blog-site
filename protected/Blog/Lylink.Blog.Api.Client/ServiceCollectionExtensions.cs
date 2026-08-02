using Lylink.Shared.Api.Client.HealthChecks;
using Lylink.Shared.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Lylink.Blog.Api.Client;

public static class ServiceCollectionExtensions
{
    extension(ClientConnectivityHealthCheck<IBlogApiClient>)
    {
        public static string NameKey => "blog-api-connectivity";
    }

    public static IServiceCollection RegisterBlogApiClient(this IServiceCollection services, BlogApiClientOptions options)
    {
        if (options.Endpoint is null)
            throw new InvalidOperationException("The endpoint for the blog API must not be null.");

        services.AddHttpClient(nameof(BlogApiClient), client =>
        {
            client.BaseAddress = new Uri(options.Endpoint);
        });

        services.AddHealthChecks()
            .AddTimeLimitedHealthCheck<ClientConnectivityHealthCheck<IBlogApiClient>>(
                ClientConnectivityHealthCheck<IBlogApiClient>.NameKey
            );

        return services.AddScoped<IBlogApiClient, BlogApiClient>();
    }
}
