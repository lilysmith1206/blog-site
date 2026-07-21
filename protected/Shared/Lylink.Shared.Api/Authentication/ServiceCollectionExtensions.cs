using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

namespace Lylink.Shared.Api.Authentication;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthentication(this IServiceCollection serviceCollection, InboundAuthenticationOptions options)
    {
        if (options.BearerToken is not null)
        {
            return serviceCollection.SetUpJwtBearerAuthentication(options.BearerToken);
        }
        else if (options.OpenIdConnect is not null)
        {
            return serviceCollection.SetUpOpenIdConnectAuthentication(options.OpenIdConnect);
        }
        else
        {
            throw new InvalidOperationException("Inbound authentication options does not have any valid authentication configuration.");
        }
    }

    private static IServiceCollection SetUpJwtBearerAuthentication(this IServiceCollection serviceCollection, BearerTokenValidationOptions bearerOptions)
    {
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

    private static IServiceCollection SetUpOpenIdConnectAuthentication(this  IServiceCollection services, OpenIdConnectOptions openIdOptions)
    {
        if (openIdOptions.ExpectedAuthority is null)
                throw new InvalidOperationException("Expected authority cannot be null for OpenID Connect authentication.");

#if RELEASE
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedProto |
                ForwardedHeaders.XForwardedHost;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });
#endif

        Uri openIdAuthority = new Uri(openIdOptions.ExpectedAuthority, UriKind.Absolute);

        if (openIdOptions.RequiresHttpsAuthority == true && openIdAuthority.Scheme == Uri.UriSchemeHttp)
            throw new InvalidOperationException("Expected authority cannot be http when https is required for OpenID Connect authentication.");

        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
        })
        .AddCookie()
        .AddOpenIdConnect(options =>
        {
            options.Authority = openIdOptions.ExpectedAuthority;
            options.ClientId = openIdOptions.ClientId;
            options.ClientSecret = openIdOptions.ClientSecret;
            options.ResponseType = "code";

            options.SaveTokens = true;
            options.RequireHttpsMetadata = openIdOptions.RequiresHttpsAuthority ?? false;
     
            options.Scope.Clear();

            options.Scope.Add("profile");
            options.Scope.Add("openid");

            foreach (var scope in openIdOptions.RequiredScopes)
                options.Scope.Add(scope);
        });

        return services;
    }
}
