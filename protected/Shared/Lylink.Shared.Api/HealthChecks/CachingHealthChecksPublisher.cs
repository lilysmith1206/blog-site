using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace Lylink.Shared.Api.HealthChecks;

public class CachingHealthChecksPublisher : IHealthCheckPublisher
{
    private readonly ILogger<CachingHealthChecksPublisher> _logger;
    private readonly HealthReportWrapper _wrapper;

    public CachingHealthChecksPublisher(
        ILogger<CachingHealthChecksPublisher> logger,
        HealthReportWrapper wrapper
    )
    {
        _logger = logger;
        _wrapper = wrapper;
    }

    public async Task PublishAsync(HealthReport report, CancellationToken cancellationToken)
    {
        using var _ = _logger.BeginScope(new { Operation = "UpdateHealthReportCache" });

        _logger.LogTrace("Updating cache of the health report.");
        _wrapper.UpdateReference(report);
        _logger.LogDebug("Wrapper reference updated to latest report.");
    }

    public record HealthReportWrapper()
    {
        public HealthReport? Report { get; private set; }

        public void UpdateReference(HealthReport report) => Report = report;
    }
}
