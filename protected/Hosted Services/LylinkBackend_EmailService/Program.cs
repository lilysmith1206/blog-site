using LylinkBackend_DatabaseAccessLayer.Models;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkBackend_EmailService.Models;
using LylinkBackend_EmailService.Services;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IVisitAnalyticsRepository, VisitAnalyticsRepository>();
            
            builder.Services.AddHostedService<EmailServiceWorker>();

            builder.Services.Configure<EmailOptions>(
                builder.Configuration.GetSection("EmailOptions"));

            var host = builder.Build();
            host.Run();
        }
    }
}