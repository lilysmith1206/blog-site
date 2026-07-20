using Lylink.Database.Repositories.Analytics;
using Lylink.Database.Context.Models;
using Lylink.Workers.Email.Models;
using Lylink.Workers.Email.Services;
using Microsoft.EntityFrameworkCore;
using Resend;

namespace Lylink.Workers.Email
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddDbContextFactory<LylinkdbContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("MariaDbConnection"), ServerVersion.Parse("11.5.2-mariadb"));
            });

            EmailOptions? emailConfiguration = builder.Configuration.GetSection("Email").Get<EmailOptions>();

            if (emailConfiguration == null)
            {
                throw new InvalidOperationException("Email block must be configured and valid for the Email Service to function. Validate configuration and restart.");
            }

            ConfigureEmailProvider(builder, emailConfiguration);

            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IVisitAnalyticsRepository, VisitAnalyticsRepository>();

            builder.Services.AddHostedService<EmailServiceWorker>();

            builder.Services.AddOptions<EmailOptions>()
                .Bind(builder.Configuration.GetSection("Email"));

            var host = builder.Build();

            host.Run();
        }

        private static void ConfigureEmailProvider(HostApplicationBuilder builder, EmailOptions emailConfiguration)
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