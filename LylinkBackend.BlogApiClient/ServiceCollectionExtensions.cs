using Microsoft.Extensions.DependencyInjection;

namespace LylinkBackend.BlogApiClient;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBlogApiClient(this IServiceCollection services, BlogApiClientOptions options)
    {
        if (options.Endpoint is null)
            throw new InvalidOperationException("The endpoint for the blog API must not be null.");

        services.AddHttpClient(nameof(BlogApiClient), client =>
        {
            client.BaseAddress = new Uri(options.Endpoint);
        });

        return services.AddScoped<IBlogApiClient, BlogApiClient>();
    }
}
