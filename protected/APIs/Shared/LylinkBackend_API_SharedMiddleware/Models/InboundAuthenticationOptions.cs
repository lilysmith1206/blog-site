namespace LylinkBackend_API_Shared.Models;

public record class InboundAuthenticationOptions
{
    public string? Authority { get; set; }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }
}
