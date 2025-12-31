
namespace LylinkBackend.AnalyticsShared;

public class NewFailureVisitAnalytic
{
    public required string Attempted { get; set; }

    public required string RedirectedTo { get; set; }

    public required DateTime Date { get; set; }
    
    public required string SessionId { get; set; }
}
