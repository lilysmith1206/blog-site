namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class VisitAnalytic
{
    public int Id { get; set; }

    public string VisitorId { get; set; } = null!;

    public string SlugVisited { get; set; } = null!;

    public string SlugGiven { get; set; } = null!;

    public DateTime VisitedOn { get; set; }
}
