using System;
using System.Collections.Generic;

namespace LylinkBackend_Database.Models;

public partial class PostHierarchy
{
    public string CategoryId { get; set; } = null!;

    public string? ParentId { get; set; }

    public string? Name { get; set; }

    public string? Slug { get; set; }

    public string? Title { get; set; }

    public string? Keywords { get; set; }

    public string? Description { get; set; }

    public string? Body { get; set; }

    public bool? UseDateCreatedForSorting { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
