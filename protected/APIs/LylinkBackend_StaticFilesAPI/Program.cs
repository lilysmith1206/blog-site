
using Microsoft.Extensions.FileProviders;

namespace LylinkBackend_StaticFilesAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
            RequestPath = "",
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=31536000");
                ctx.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            }
        });

        app.MapControllers();

        app.Run();
    }
}
