
using LylinkBackend_API_Shared.Middleware;
using LylinkBackend_API_Shared.Models;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkBackend_ManagementAPI.Middleware;
using LylinkBackend_ManagementAPI.Models;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
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

        builder.Services.Configure<AssetsOriginOptions>(
            builder.Configuration.GetSection("AssetsOriginOptions"));

        builder.Services.Configure<MainSiteOptions>(
            builder.Configuration.GetSection("MainSiteOptions"));

        builder.Services.Configure<AuthenticationOptions>(
            builder.Configuration.GetSection("AuthenticationOptions"));

        builder.Services.AddDbContext<LylinkdbContext>(options =>
        {
            options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
        });

        builder.Services.AddTransient<IPageManagementRepository, PageManagementRepository>();

        builder.Services.AddControllers();
        builder.Services.AddRazorPages();
        builder.Services.AddRazorComponents();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors(AssetsOrigins);

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

        app.UseMiddleware<RetrieveStaticAssetMiddleware>();
        app.UseMiddleware<CertificateValidationMiddleware>();

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
