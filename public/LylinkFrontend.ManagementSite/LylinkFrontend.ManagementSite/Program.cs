
using LylinkBackend.ManagementApiClient;
using LylinkBackend_API_Shared.Middleware;
using LylinkBackend_API_Shared.Models;
using LylinkFrontend.ManagementSite.Models;

namespace LylinkFrontend.ManagementSite;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        const string AssetsOrigins = "_assetsOrigin";

        string assetUrl = builder.Configuration.GetSection("AssetsOriginOptions").GetValue<string>("AssetsEndpointHttps")
            ?? throw new NullReferenceException("No assets https endpoint configured.");

        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: AssetsOrigins, policy =>
            {
                policy.WithOrigins(assetUrl)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .WithExposedHeaders("Content-Disposition")
                  .SetIsOriginAllowedToAllowWildcardSubdomains()
                  .AllowCredentials();
            });
        });

        builder.Services.Configure<AssetsOriginOptions>(
            builder.Configuration.GetSection("AssetsOriginOptions"));

        builder.Services.Configure<MainSiteOptions>(
            builder.Configuration.GetSection("MainSiteOptions"));

        var options = builder.Configuration.GetSection("ManagementApiClient")
            .Get<ManagementApiClientOptions>();

        if (options is null)
            throw new NullReferenceException("Management API client settings must be present.");

        builder.Services.RegisterManagementApiClient(options);

        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddRazorComponents();

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseCors(AssetsOrigins);
        app.MapControllers();
        app.MapRazorPages();

        app.UseMiddleware<RetrieveStaticAssetMiddleware>();

        app.Use(async (context, next) =>
        {
            if (context.Request.Method == "OPTIONS")
            {
                context.Response.Headers.Append("Access-Control-Allow-Origin", assetUrl);
                context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, OPTIONS");
                context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
                context.Response.StatusCode = 204;

                return;
            }

            await next();
        });

        app.Run();
    }
}
