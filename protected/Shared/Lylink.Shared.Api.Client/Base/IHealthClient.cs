using ErrorOr;

namespace Lylink.Shared.Api.Client.Base;

public interface IHealthClient
{
    Task<ErrorOr<Success>> GetHealth(CancellationToken cancellationToken = default);
}
