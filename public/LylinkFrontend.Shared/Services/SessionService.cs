namespace LylinkFrontend.Shared.Services;

public class SessionService : ISessionService
{
    private readonly string _sessionId;

    public SessionService()
    {
        _sessionId = $"{Guid.NewGuid()} - {DateTime.UtcNow}";
    }

    public string SessionId { get => _sessionId; }
}
