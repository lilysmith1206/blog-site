using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Lylink.Shared.HealthChecks;

public static class ServiceCollectionExtensions
{
    private static TimeSpan TimeoutSpan => TimeSpan.FromSeconds(2);

    extension(IHealthChecksBuilder builder)
    {
        public IHealthChecksBuilder AddTimeLimitedHealthCheck<THealthCheck>(string name, HealthStatus failureStatus = HealthStatus.Unhealthy)
            where THealthCheck : class, IHealthCheck
        {
            return builder.AddCheck<THealthCheck>(name, timeout: TimeoutSpan);
        }

        public IHealthChecksBuilder AddTimeLimitedTypeActivatedHealthCheck<THealthCheck>(string name, object[]? args = null)
            where THealthCheck : class, IHealthCheck
        {
            return builder.AddTypeActivatedCheck<THealthCheck>(
                name,
                failureStatus: HealthStatus.Unhealthy,
                timeout: TimeoutSpan,
                tags: [],
                args: args ?? []
            );
        }
    }
}