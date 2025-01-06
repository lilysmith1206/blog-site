using System;
using System.Collections.Generic;

namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class Post
{
    public string Slug { get; set; } = null!;

    public string? Title { get; set; }

    public int? ParentId { get; set; }

    public DateTime? DateModified { get; set; }

    public string? Name { get; set; }

    public string? Keywords { get; set; }

    public string? Description { get; set; }

    public string? Body { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual PostCategory? Parent { get; set; }
}
