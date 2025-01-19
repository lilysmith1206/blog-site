namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class Post
{
    public int Id { get; set; }

    public string Slug { get; set; } = null!;

    public int? ParentId { get; set; }

    public DateTime DateModified { get; set; }

    public DateTime DateCreated { get; set; }

    public bool IsDraft { get; set; }

    public virtual PostCategory? Parent { get; set; }

    public virtual Page SlugNavigation { get; set; } = null!;
}
