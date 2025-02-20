using LylinkBackend_API.Caches;
using LylinkBackend_API.Middleware;
using LylinkBackend_API.Models;
using LylinkBackend_API.Services;
using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using Microsoft.EntityFrameworkCore;

#if RELEASE
using System.Security.Cryptography.X509Certificates;
#endif

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

            builder.Services.Configure<EmailOptions>(
                builder.Configuration.GetSection("Email"));

            builder.Services.AddTransient<IAnnotationRepository, AnnotationsRepository>();
            builder.Services.AddTransient<IPageRepository, PageRepository>();
            builder.Services.AddTransient<IPageManagementRepository, PageManagementRepository>();
            builder.Services.AddTransient<IVisitAnalyticsRepository, VisitAnalyticsRepository>();
            builder.Services.AddTransient<ISlugRepository, SlugRepository>();
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IStylesheetManagementRepository, StylesheetManagementRepository>();

            builder.Services.AddSingleton<IUserCookieService, UserCookieService>();
            builder.Services.AddSingleton<ISlugCache, SlugCache>();
            builder.Services.AddSingleton<IVisitAnalyticsCache, VisitAnalyticsCache>();

            builder.Services.AddHostedService<SlugDataService>();
            builder.Services.AddHostedService<VisitAnalyticsProcessingService>();
            builder.Services.AddHostedService<VisitAnalyticsEmailService>();

            builder.Services.AddControllers();
            builder.Services.AddRazorPages();
            builder.Services.AddRazorComponents();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=31536000");
                }
            });

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.UseMiddleware<CreateVisitIdMiddleware>();
            app.UseMiddleware<TokenValidationMiddleware>();

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
