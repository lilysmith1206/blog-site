using ErrorOr;
using LylinkBackend.BlogShared.Page;
using LylinkShared.Models;

namespace LylinkBackend.BlogApiClient;

public interface IBlogApiClient
{
    Task<ErrorOr<Page>> GetIndexPage(CancellationToken cancellationToken = default);

    Task<ErrorOr<Page>> GetNotFoundPage(CancellationToken cancellationToken = default);

    Task<ErrorOr<List<PageLink>>> GetMostRecentPosts(int limit, CancellationToken cancellationToken = default);

    Task<ErrorOr<Page>> GetPageFromSlug(string slug, CancellationToken cancellationToken = default);
}