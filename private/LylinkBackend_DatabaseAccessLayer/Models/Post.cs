namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class Post
{
    public string Slug { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int? ParentId { get; set; }

    public DateTime DateModified { get; set; }

    public string Name { get; set; } = null!;

    public string Keywords { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Body { get; set; } = null!;

    public DateTime DateCreated { get; set; }

    public bool IsDraft { get; set; }

    public virtual PostCategory? Parent { get; set; }
}
