
using LylinkBackend_API_Shared.Middleware;
using LylinkBackend_API_Shared.Models;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkBackend_ManagementAPI.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_ManagementAPI;

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

        var authenticationOptions = builder.Configuration
            .GetSection(nameof(AuthenticationOptions))
            .Get<AuthenticationOptions>();

        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.Authority = "http://localhost:8080/realms/lylink/";
                options.ClientId = "lylink_management";
                options.ClientSecret = "gfcPHBarzdo5aq9hHc3XIqwmAEavueOI";
                options.ResponseType = "code";

                options.SaveTokens = true;

                options.RequireHttpsMetadata = false; // localhost only
            });

        builder.Services.AddAuthorization();

        builder.Services.Configure<AssetsOriginOptions>(
            builder.Configuration.GetSection("AssetsOriginOptions"));

        builder.Services.Configure<MainSiteOptions>(
            builder.Configuration.GetSection("MainSiteOptions"));

        builder.Services.AddDbContext<LylinkdbContext>(options =>
        {
            options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
        });

        builder.Services.AddTransient<IPageManagementRepository, PageManagementRepository>();

        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddRazorComponents();

        var app = builder.Build();

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpsRedirection();

        app.UseCors(AssetsOrigins);
        app.MapControllers();
        app.MapRazorPages().RequireAuthorization();

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
