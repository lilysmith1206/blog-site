using Microsoft.Extensions.DependencyInjection;

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
            => builder.AddCheck<LylinkDatabaseHealthCheck>(name);
    }
}