using LylinkBackend_API.Caches;
using LylinkBackend_API.Middleware;
using LylinkBackend_API.Services;
using LylinkBackend_API_Shared.Middleware;
using LylinkBackend_API_Shared.Models;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddPooledDbContextFactory<LylinkdbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
            });

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

            builder.Services.AddTransient<IAnnotationRepository, AnnotationsRepository>();
            builder.Services.AddTransient<IPageRepository, PageRepository>();
            builder.Services.AddTransient<IVisitAnalyticsRepository, VisitAnalyticsRepository>();
            builder.Services.AddTransient<ISlugRepository, SlugRepository>();

            builder.Services.AddSingleton<IUserCookieService, UserCookieService>();
            builder.Services.AddSingleton<ISlugCache, SlugCache>();
            builder.Services.AddSingleton<IVisitAnalyticsCache, VisitAnalyticsCache>();

            builder.Services.AddHostedService<SlugDataService>();
            builder.Services.AddHostedService<VisitAnalyticsProcessingService>();

            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
            builder.Services.AddRazorComponents();

            var app = builder.Build();

            app.UseCors(AssetsOrigins);

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

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

            app.UseStaticFiles();

            app.UseMiddleware<CreateVisitIdMiddleware>();
            app.UseMiddleware<RetrieveStaticAssetMiddleware>();

            app.MapControllers();

            app.MapControllerRoute(
                name: "root",
                pattern: "",
                defaults: new { controller = "Page", action = "RenderPage", slug = "/" }
            );

            app.MapControllerRoute(
                name: "fallback",
                pattern: "{**slug}",
                defaults: new { controller = "Page", action = "RenderPage" },
                constraints: new { slug = @"^(?!swagger|css|js|lib|images|favicon.ico|robots.txt).*" } // Exclude static file paths
            );

            app.Run();
        }
    }
}
