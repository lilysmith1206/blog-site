using ErrorOr;
using LylinkBackend.AnalyticsShared;

namespace LylinkBackend.Analytics.Client;

public interface IAnalyticsApiClient
{
    Task<ErrorOr<int>> CreateFailureVisitAnalytic(NewFailureVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default);
    
    Task<ErrorOr<int>> CreateSuccessVisitAnalytic(NewSuccessVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default);
}