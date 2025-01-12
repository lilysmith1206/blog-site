namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class VisitAnalytic
{
    public int Id { get; internal set; }

    public string? VisitorId { get; set; }

    public string? SlugVisited { get; set; }

    public string? SlugGiven { get; set; }

    public DateTime? VisitedOn { get; set; }
}
