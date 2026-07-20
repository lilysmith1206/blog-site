using Lylink.Database.Context.Models;

namespace Lylink.Database.Repositories.Models;

public class Analytics
{
    public required List<VisitAnalytic> SuccessVisits { get; init; }

    public required List<FailedVisitAnalytic> FailedVisit { get; init; }
}
