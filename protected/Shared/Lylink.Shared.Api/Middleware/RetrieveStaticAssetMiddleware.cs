using Lylink.Shared.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Lylink.Shared.Api.Middleware
{
    public class RetrieveStaticAssetMiddleware(RequestDelegate next, IOptions<AssetsOriginOptions> assetsOriginOptions)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            if (assetsOriginOptions.Value.AssetsEndpointHttps == null)
            {
                throw new NullReferenceException("Asset middleware is null");
            }

            string requestPath = context.Request.Path;

            if (Regex.Match(requestPath, @".+\.\w+").Success && File.Exists(Directory.GetCurrentDirectory() + $"/wwwroot/{requestPath}") == false)
            {
                var assetUrl = $"{assetsOriginOptions.Value.AssetsEndpointHttps}{context.Request.Path}";
                    
                context.Response.Redirect(assetUrl);

                return;
            }

            await next(context);
        }
    }
}
