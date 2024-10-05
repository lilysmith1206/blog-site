using LylinkBackend.Handlers;
using LylinkBackend_API.Factories;
using LylinkBackend_API.Models;
using LylinkBackend_Database.Models;
using LylinkBackend_Database.Services;
using Microsoft.EntityFrameworkCore;
using LylinkBackend_API.Renderers;

namespace LylinkBackend
{
    public class Program
    {
        public static IEnumerable<string?> PostCategorySlugs = [];
        public static IEnumerable<string?> PostSlugs = [];

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure database context and services
            builder.Services.AddDbContext<LylinkdbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
            });

            builder.Services.AddSingleton<List<ManagementToken>>();
            builder.Services.AddTransient<IDatabaseService, DatabaseService>();
            builder.Services.AddTransient<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            // Add background slug updater
            builder.Services.AddHostedService<SlugUpdateService>();

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

            app.MapControllers();

            app.MapFallback(async (context) =>
            {
                using var scope = app.Services.CreateScope();

                IDatabaseService databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
                IRazorViewToStringRenderer renderer = scope.ServiceProvider.GetRequiredService<IRazorViewToStringRenderer>();
                string pageResult;
                string path = context.Request.Path.Value?.Trim('/') ?? string.Empty;

                bool addSuggestionTools = context.Request.Query.ContainsKey("editor");

                if (PostSlugs.Contains(path))
                {
                    PostFactory postFactory = new(databaseService, renderer);

                    pageResult = await postFactory.GeneratePost(path, addSuggestionTools);
                }
                else if (PostCategorySlugs.Contains(path))
                {
                    PostCategoryFactory postCategoryFactory = new(databaseService, renderer);

                    pageResult = await postCategoryFactory.GenerateCategoryPage(path);
                }
                else
                {
                    pageResult = string.Empty;

                    Results.Redirect("/404");
                }

                await context.Response.WriteAsync(pageResult);
            });

            app.Run();
        }
    }

    public class SlugUpdateService(IServiceProvider serviceProvider) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested == false)
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();

                    // Update slugs from the database
                    Program.PostSlugs = databaseService.GetAllPostSlugs();
                    Program.PostCategorySlugs = databaseService.GetAllCategorySlugs();
                }

                // Wait 10 seconds before updating slugs again
                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
