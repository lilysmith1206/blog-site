using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkBackend_EmailService.Models;
using LylinkBackend_EmailService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;
using Resend;

namespace LylinkBackend_EmailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContext<LylinkdbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
            });

            Email? emailConfiguration = builder.Configuration.GetSection("Email").Get<Email>();

            if (emailConfiguration == null)
            {
                throw new InvalidOperationException("Email block must be configured and valid for the Email Service to function. Validate configuration and restart.");
            }

            ConfigureEmailProvider(builder, emailConfiguration);

            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IVisitAnalyticsRepository, VisitAnalyticsRepository>();

            builder.Services.AddHostedService<EmailServiceWorker>();

            builder.Services.AddOptions<Email>()
                .Bind(builder.Configuration.GetSection("Email"));

            var host = builder.Build();

            host.Run();
        }

        private static void ConfigureEmailProvider(HostApplicationBuilder builder, Email emailConfiguration)
        {
            builder.Services.AddHttpClient<ResendClient>();
            builder.Services.Configure<ResendClientOptions>(options =>
            {
                options.ApiToken = emailConfiguration.ApiKey!;
            });

            builder.Services.AddTransient<IResend, ResendClient>();
        }
    }
}