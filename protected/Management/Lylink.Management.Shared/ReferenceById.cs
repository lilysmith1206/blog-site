namespace Lylink.Management.Shared;

public record class ReferenceById
{
    public required int Id { get; set; }

    public string? Name { get; set; }
}
