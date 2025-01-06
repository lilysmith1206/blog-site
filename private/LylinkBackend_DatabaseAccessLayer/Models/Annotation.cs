using System;
using System.Collections.Generic;

namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class Annotation
{
    public string Id { get; set; } = null!;

    public string? Slug { get; set; }

    public string? EditorName { get; set; }

    public string? AnnotationContent { get; set; }
}
