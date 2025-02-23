using LylinkBackend_ManagementAPI.Models;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;

namespace LylinkBackend_ManagementAPI.Middleware
{
    public class CertificateValidationMiddleware(RequestDelegate next, IOptions<MainSiteOptions> mainSiteOptions, IOptions<AuthenticationOptions> authenticationOptions)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            X509Certificate2? clientCertificate = await context.Connection.GetClientCertificateAsync();

            if (clientCertificate == null || IsCertificateValid(clientCertificate) == false)
            {
                context.Response.Redirect(mainSiteOptions.Value.Url + "/403");

                return;
            }

            await next(context);
        }

        private bool IsCertificateValid(X509Certificate2 certificate)
        {
            return authenticationOptions.Value.AllowedThumbprints?.Contains(certificate.Thumbprint) == true;
        }
    }
}
