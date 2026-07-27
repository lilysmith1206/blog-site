
using Lylink.Database.Repositories.Analytics;
using Lylink.Shared.Api.Authentication;
using Lylink.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Lylink.Shared.Api.HealthChecks;

namespace Lylink.Analytics.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();

        var inboundAuthentication = builder.Configuration
            .GetSection("InboundAuthentication")
            .Get<InboundAuthenticationOptions>();

        if (inboundAuthentication is null)
            throw new InvalidOperationException("Inbound authentication could not be deserialized from configuration under 'InboundAuthentication' block.");

        builder.Services.AddAuthentication(inboundAuthentication);
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("create_analytics", policy =>
            {
                policy.RequireAssertion(context =>
                {
                    var claims = context.User.Claims
                        .FirstOrDefault(claim => claim.Type == "scope");

                    return claims?.Value.Contains("analytics.create") == true;
                });
            });

        builder.Services.AddPooledDbContextFactory<LylinkdbContext>(options =>
        {
            options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
        });

        builder.Services.AddScoped<IVisitAnalyticsRepository, VisitAnalyticsRepository>();

        builder.Services.AddHealthChecks()
            .AddLylinkDatabaseHealthCheck();
            
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks("/health");
        app.MapControllers();

        app.Run();
    }
}
