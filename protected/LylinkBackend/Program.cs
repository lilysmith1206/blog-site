using LylinkBackend_API.Models;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using Microsoft.EntityFrameworkCore;
using LylinkBackend_API.Services;
using LylinkBackend_API.Caches;
using LylinkBackend_API.Middleware;

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

            builder.Services.AddDbContext<LylinkdbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
            });

            builder.Services.AddTransient<IAnnotationDatabaseService, DatabaseService>();
            builder.Services.AddTransient<IPostDatabaseService, DatabaseService>();
            builder.Services.AddTransient<IPostCategoryDatabaseService, DatabaseService>();

            builder.Services.AddSingleton<ISlugCache, SlugCache>();
            builder.Services.AddSingleton<IAccessTokenCache, AccessTokenCache>();

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
