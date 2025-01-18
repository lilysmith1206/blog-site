namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class DatabaseVersion
{
    public string Version { get; set; } = null!;

    public DateTime UpdatedOn { get; set; }
}
