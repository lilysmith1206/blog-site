using Microsoft.EntityFrameworkCore;
using LylinkBackend_API.Services;
using LylinkBackend_API.Caches;
using LylinkBackend_API.Middleware;
using LylinkBackend_API.Models;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using System.Security.Cryptography.X509Certificates;

namespace LylinkBackend
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

#if DEBUG
#else
            builder.WebHost.ConfigureKestrel((context, options) =>
            {
                var certPath = context.Configuration["Kestrel:EndPoints:Https:Certificate:Path"] ?? throw new NullReferenceException("Certificate path is null");
                var keyPath = context.Configuration["Kestrel:EndPoints:Https:Certificate:KeyPath"] ?? throw new NullReferenceException("Private key path is null");

                string key = File.ReadAllText(keyPath);

                X509Certificate2 certificate = new(certPath, key, X509KeyStorageFlags.Exportable);

                options.ConfigureHttpsDefaults(listenOptions =>
                {
                    listenOptions.ServerCertificate = certificate;
                });
            });
#endif
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<LylinkdbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
            });

            builder.Services.Configure<Authentication>(
                builder.Configuration.GetSection("Authentication"));

            builder.Services.AddTransient<IAnnotationDatabaseService, DatabaseService>();
            builder.Services.AddTransient<IPostDatabaseService, DatabaseService>();
            builder.Services.AddTransient<IPostCategoryDatabaseService, DatabaseService>();

            builder.Services.AddSingleton<ISlugCache, SlugCache>();

            builder.Services.AddHostedService<SlugDataService>();

            builder.Services.AddControllers();
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseMiddleware<TokenValidationMiddleware>();

            app.MapControllers();

            app.MapControllerRoute(
                name: "root",
                pattern: "",
                defaults: new { controller = "Page", action = "RenderPage", slug = "" }
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
