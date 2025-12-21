using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;

namespace LylinkBackend.SharedApiCode.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection serviceCollection, InboundAuthenticationOptions options)
    {
        if (options.BearerTokenValidation is not null)
        {
            var bearerOptions = options.BearerTokenValidation;

            if (bearerOptions.ExpectedAuthority is null)
                throw new InvalidOperationException("Expected authority cannot be null for JWT bearer authentication.");

            if (bearerOptions.ValidateAudience == true && bearerOptions.ValidateAudience is null)
                throw new InvalidOperationException("Expected audience cannot be null when validating audience for JWT bearer authentication.");

            Uri bearerAuthority = new Uri(bearerOptions.ExpectedAuthority, UriKind.Absolute);

            if (bearerOptions.RequiresHttpsAuthority == true && bearerAuthority.Scheme == Uri.UriSchemeHttp)
                throw new InvalidOperationException("Expected authority cannot be http when https is required for JWT bearer authentication.");

            serviceCollection
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(jwtOptions =>
                {
                    jwtOptions.Authority = bearerOptions.ExpectedAuthority;
                    jwtOptions.Audience = bearerOptions.ExpectedAudience;
                    jwtOptions.RequireHttpsMetadata = bearerOptions.RequiresHttpsAuthority ?? false;

                    jwtOptions.TokenValidationParameters = new()
                    {
                        ValidateIssuer = bearerOptions.ValidateIssuer ?? false,
                        ValidateAudience = bearerOptions.ValidateAudience ?? false,
                        ValidateLifetime = bearerOptions.ValidateLifetime ?? false
                    };
                });

            return serviceCollection;
        }
        else
        {
            throw new InvalidOperationException("Inbound authentication options does not have any valid authentication configuration.");
        }
    }
}
