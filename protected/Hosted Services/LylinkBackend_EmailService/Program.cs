using LylinkBackend_EmailService.Models;
using LylinkBackend_EmailService.Services;

namespace LylinkBackend_EmailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddHostedService<EmailServiceWorker>();

            builder.Services.Configure<EmailOptions>(
                builder.Configuration.GetSection("EmailOptions"));

            var host = builder.Build();
            host.Run();
        }
    }
}