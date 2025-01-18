namespace LylinkBackend_DatabaseAccessLayer.Models;

public partial class Annotation
{
    public string Id { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public string EditorName { get; set; } = null!;

    public string AnnotationContent { get; set; } = null!;
}
