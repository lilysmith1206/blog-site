namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class Page
{
    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Keywords { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Body { get; set; } = null!;

    public virtual Post? Post { get; set; }

    public virtual ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}
