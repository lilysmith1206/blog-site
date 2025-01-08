using System;
using System.Collections.Generic;

namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class PostCategory
{
    public int CategoryId { get; set; }

    public int? ParentId { get; set; }

    public string? CategoryName { get; set; }

    public string? Slug { get; set; }

    public string? Title { get; set; }

    public string? Keywords { get; set; }

    public string? Description { get; set; }

    public string? Body { get; set; }

    public bool? UseDateCreatedForSorting { get; set; }

    public virtual ICollection<PostCategory> InverseParent { get; set; } = new List<PostCategory>();

    public virtual PostCategory? Parent { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
