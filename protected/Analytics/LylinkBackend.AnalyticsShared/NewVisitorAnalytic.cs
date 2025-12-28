
namespace LylinkBackend.AnalyticsShared;

public class NewVisitorAnalytic
{
    public required string VisitTarget { get; set; }

    public required string VisitResult { get; set; }
    
    public DateTime Date { get; set; }
}
