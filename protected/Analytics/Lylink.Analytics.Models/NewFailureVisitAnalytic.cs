
namespace Lylink.Analytics.Models;

public class NewSuccessVisitAnalytic
{
    public required string Visited { get; set; }
    
    public required DateTime Date { get; set; }
    
    public required string SessionId { get; set; }
}
