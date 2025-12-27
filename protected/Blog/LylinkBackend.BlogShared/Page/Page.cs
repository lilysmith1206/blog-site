using LylinkShared.Models;

namespace LylinkBackend.BlogShared.Page;

public class Page
{
    public List<PageLink> Parents { get; set; } = [];

    public List<PageLink> Children { get; set; } = [];

    public List<PageLink> Posts { get; set; } = [];

    public string? Name { get; set; }

    public string? HtmlContent { get; set; }

    public Metadata? Metadata { get; set; }
}
