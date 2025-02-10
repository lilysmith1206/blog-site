using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Mappers
{
    public static class PostMapper
    {
        public static void Map(this Post post, IEnumerable<PageLink> parents, out PostPage page)
        {
            Page databasePage = post.SlugNavigation;

            page = new PostPage
            {
                Body = databasePage.Body,
                DateCreated = post.DateCreated,
                DateModified = post.DateModified,
                Description = databasePage.Description,
                IsDraft = post.IsDraft,
                Keywords = databasePage.Keywords,
                Name = databasePage.Name,
                Parents = parents,
                Slug = databasePage.Slug,
                Title = databasePage.Title,
            };
        }

        public static void Map(this Post post, out PostInfo postInfo)
        {
            postInfo = new PostInfo
            {
                Id = post.Id,
                Body = post.SlugNavigation.Body,
                Description = post.SlugNavigation.Description,
                Keywords = post.SlugNavigation.Keywords,
                Name = post.SlugNavigation.Name,
                ParentId = post.ParentId,
                Slug = post.SlugNavigation.Slug,
                Title = post.SlugNavigation.Title,
                IsDraft = post.IsDraft
            };
        }
    }
}
