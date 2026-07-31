using Lylink.Shared.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lylink.Shared.Api.HealthChecks;

public static class ServiceCollectionExtensions
{
    extension(IHealthChecksBuilder builder)
    {
        /// <summary>
        /// Adds the <see cref="LylinkDatabaseHealthCheck"/> as a health check to the builder.
        /// </summary>
        /// <param name="name">The name to use for the health check.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/> for further calls.</returns>
        public IHealthChecksBuilder AddLylinkDatabaseHealthCheck(string name = "lylink-database-connectivity")
            => builder.AddTimeLimitedHealthCheck<LylinkDatabaseHealthCheck>(name);

        /// <summary>
        /// Adds the <see cref="InboundKeycloakConnectivityHealthCheck"/> to the builder.
        /// </summary>
        /// <remarks>
        /// This <strong>requires</strong> an <see cref="IHttpClientFactory"/> to be available as part of the DI container.
        /// </remarks>
        /// <param name="name">The name to use for the health check.</param>
        /// <param name="healthStatus">The health status for this to be, if the check finds it is unhealthy.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/> for further calls.</returns>
        public IHealthChecksBuilder AddInboundKeycloakConnectivityHealthCheck(HealthStatus healthStatus = HealthStatus.Unhealthy, string name = "oauth2-connectivity")
            => builder.AddTimeLimitedHealthCheck<InboundKeycloakConnectivityHealthCheck>(name, healthStatus);
    }
}