using Lylink.Analytics.Api.Client;
using Lylink.Blog.Api.Client;
using Lylink.Site.Blog.Components;
using Lylink.Shared.Api.Models;
using Lylink.Site.Shared.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lylink.Site.Blog;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddOptions<AssetsOriginOptions>()
            .BindConfiguration("AssetsOriginOptions");

        var blogOptions = builder.Configuration.GetSection("BlogApiClient")
            .Get<BlogApiClientOptions>();

        if (blogOptions is null)
            throw new NullReferenceException("Blog API client settings must be present.");

        var analyticsOptions = builder.Configuration.GetSection("AnalyticsApiClient")
            .Get<AnalyticsApiClientOptions>();

        if (analyticsOptions is null)
            throw new NullReferenceException("Analytics API client settings must be present.");

        builder.Services.RegisterAnalyticsApiClient(analyticsOptions)
                        .RegisterBlogApiClient(blogOptions);
                        
        builder.Services.AddScoped<ISessionService, SessionService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.MapStaticAssets();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.MapHealthChecks("/api/health");
        
        app.Run();
    }
}
