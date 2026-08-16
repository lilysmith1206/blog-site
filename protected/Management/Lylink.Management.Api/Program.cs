
using Lylink.Database.Repositories.Management;
using Lylink.Shared.Api.Authentication;
using Lylink.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Lylink.Shared.Api.HealthChecks;

namespace Lylink.Management.Api;

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

        var inboundAuthentication = builder.Configuration
            .GetSection("InboundAuthentication")
            .Get<InboundAuthenticationOptions>();

        if (inboundAuthentication is null)
            throw new InvalidOperationException("Inbound authentication could not be deserialized from configuration under 'InboundAuthentication' block.");

        builder.Services.AddAuthentication(inboundAuthentication);
        builder.Services.AddAuthorization();

        builder.Services.AddPooledDbContextFactory<LylinkdbContext>(options =>
        {
            options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
        });

        builder.Services.AddTransient<IPageManagementRepository, PageManagementRepository>();
        builder.Services.AddHttpClient();

        builder.Services
            .AddCachingHealthCheckPublisher()
            .AddHealthChecks()
            .AddLylinkDatabaseHealthCheck()
            .AddInboundKeycloakConnectivityHealthCheck();

        var app = builder.Build();
    
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health");
        app.MapControllers();

        app.Run();
    }
}
