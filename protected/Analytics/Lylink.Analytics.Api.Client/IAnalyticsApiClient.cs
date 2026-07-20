using ErrorOr;
using Lylink.Analytics.Models;

namespace Lylink.Analytics.Api.Client;

public interface IAnalyticsApiClient
{
    Task<ErrorOr<int>> CreateFailureVisitAnalytic(NewFailureVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default);
    
    Task<ErrorOr<int>> CreateSuccessVisitAnalytic(NewSuccessVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default);
}