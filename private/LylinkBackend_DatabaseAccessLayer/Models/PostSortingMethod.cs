namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class PostSortingMethod
{
    public int Id { get; set; }

    public string SortingName { get; set; } = null!;

    public virtual ICollection<PostCategory> PostCategories { get; set; } = new List<PostCategory>();
}
