namespace Lylink.Shared.Models;

public record ServiceUnavailableResponse
{
    public List<string> FailedChecks { get; init; } = [];
}
