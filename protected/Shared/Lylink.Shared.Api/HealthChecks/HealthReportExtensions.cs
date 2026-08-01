using Microsoft.Extensions.Diagnostics.HealthChecks;
using static Lylink.Shared.Api.HealthChecks.CachingHealthChecksPublisher;

namespace Lylink.Shared.Api.HealthChecks;

public static class HealthReportExtensions
{
    extension(HealthReportWrapper wrapper)
    {
        public bool IsCheckUnhealthy(string checkName)
        {
            var entry = wrapper.GetCheckByName(checkName);

            return entry is not null && entry.Value.Status != HealthStatus.Healthy;
        }

        private HealthReportEntry? GetCheckByName(string checkName)
        {
            if (wrapper.Report is null)
                throw new ArgumentException("Report is not set, this cannot be executed.");

            _ = wrapper.Report.Entries.TryGetValue(checkName, out var entry);

            return entry;
        }
    }
}
