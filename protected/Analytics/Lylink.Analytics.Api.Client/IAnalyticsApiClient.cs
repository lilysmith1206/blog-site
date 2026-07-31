using ErrorOr;
using Lylink.Analytics.Models;
using Lylink.Shared.Api.Client.Base;

namespace Lylink.Analytics.Api.Client;

public interface IAnalyticsApiClient : IHealthClient
{
    Task<ErrorOr<int>> CreateFailureVisitAnalytic(NewFailureVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default);
    
    Task<ErrorOr<int>> CreateSuccessVisitAnalytic(NewSuccessVisitAnalytic visitorAnalytic, CancellationToken cancellationToken = default);
}