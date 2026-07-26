using ErrorOr;
using Lylink.Management.Shared;
using Lylink.Shared.Models;

namespace Lylink.Management.Api.Client;

public interface IManagementApiClient
{
    Task<ErrorOr<Dictionary<string, List<ReferenceById>>>> GetPostsByCategoryAsync(CancellationToken cancellationToken = default);

    Task<ErrorOr<List<ReferenceById>>> GetCategoriesAsync(CancellationToken cancellationToken = default);

    Task<ErrorOr<PostInfo>> GetPostById(int id, CancellationToken cancellationToken = default);

    Task<ErrorOr<CategoryInfo>> GetCategoryById(int id, CancellationToken cancellationToken = default);

    Task<ErrorOr<int>> CreatePost(PostInfo post, CancellationToken cancellationToken = default);

    Task<ErrorOr<int>> CreateCategory(CategoryInfo category, CancellationToken cancellationToken = default);

    Task<ErrorOr<Success>> UpdatePost(int id, PostInfo post, CancellationToken cancellationToken = default);

    Task<ErrorOr<Success>> UpdateCategory(int id, CategoryInfo category, CancellationToken cancellationToken = default);
}