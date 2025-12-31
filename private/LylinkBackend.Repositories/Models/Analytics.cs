using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend.Repositories.Models;

public class Analytics
{
    public required List<VisitAnalytic> SuccessVisits { get; init; }

    public required List<FailedVisitAnalytic> FailedVisit { get; init; }
}
