
using Lylink.Database.Repositories.Pages;
using Lylink.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Lylink.Shared.Api.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lylink.Blog.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddPooledDbContextFactory<LylinkdbContext>(options =>
        {
            options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
        });
        builder.Services.AddTransient<IPageRepository, PageRepository>();

        builder.Services
            .AddCachingHealthCheckPublisher()
            .AddHealthChecks()
            .AddLylinkDatabaseHealthCheck();

        var app = builder.Build();
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health");
        app.MapControllers();

        app.Run();
    }
}
