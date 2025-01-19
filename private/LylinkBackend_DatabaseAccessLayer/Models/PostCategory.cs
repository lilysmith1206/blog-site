namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class PostCategory
{
    public int CategoryId { get; set; }

    public int? ParentId { get; set; }

    public string Slug { get; set; } = null!;

    public bool UseDateCreatedForSorting { get; set; }

    public virtual ICollection<PostCategory> InverseParent { get; set; } = new List<PostCategory>();

    public virtual PostCategory? Parent { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual Page SlugNavigation { get; set; } = null!;
}
