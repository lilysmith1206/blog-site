
using Lylink.Database.Repositories.Analytics;
using Lylink.Shared.Api.Authentication;
using Lylink.Database.Context.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
        builder.Services.AddSwaggerGen(opt =>
        {
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        []
                    }
                });
        });

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


        app.MapControllers();

        app.Run();
    }
}
