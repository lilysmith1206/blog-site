using ErrorOr;
using Lylink.Blog.Shared.Page;
using Lylink.Shared.Models;

namespace Lylink.Blog.Api.Client;

public interface IBlogApiClient
{
    Task<ErrorOr<Page>> GetIndexPage(CancellationToken cancellationToken = default);

    Task<ErrorOr<Page>> GetNotFoundPage(CancellationToken cancellationToken = default);

    Task<ErrorOr<List<PageLink>>> GetMostRecentPosts(int limit, CancellationToken cancellationToken = default);

    Task<ErrorOr<Page>> GetPageFromSlug(string slug, CancellationToken cancellationToken = default);
}