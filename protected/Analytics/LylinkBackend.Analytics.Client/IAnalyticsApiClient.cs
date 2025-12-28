using ErrorOr;
using LylinkBackend.AnalyticsShared;

namespace LylinkBackend.Analytics.Client;

public interface IAnalyticsApiClient
{
    Task<ErrorOr<int>> CreateVisitorAnalytic(NewVisitorAnalytic visitorAnalytic, CancellationToken cancellationToken = default);
}