using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class PageRepository(LylinkdbContext context) : IPageRepository
    {
        private readonly PostCategory IndexCategory = context.PostCategories.Single(category => category.Slug == "/");

        public IEnumerable<KeyValuePair<string, string>> GetRecentlyUpdatedPostInfos(int amount)
        {
            return context.Posts
                .OrderByDescending(post => post.DateModified)
                .Where(post => post.IsDraft == false)
                .Take(amount)
                .Include(post => post.SlugNavigation)
                .Select(post => KeyValuePair.Create(post.Slug, post.SlugNavigation.Name));
        }

        public PostPage? GetPost(int id)
        {
            Post? databasePost = context.Posts
                .Where(post => post.Id == id)
                .Include(post => post.SlugNavigation)
                .Include(post => post.Parent)
                    .ThenInclude(parent => parent!.SlugNavigation)
                .SingleOrDefault();

            if (databasePost == null)
            {
                return null;
            }

            return ConvertPostToPostPage(databasePost);
        }

        public PostPage? GetPost(string slug)
        {
            Post? databasePost = context.Posts
                .Where(post => post.Slug == slug)
                .Include(post => post.SlugNavigation)
                .Include(post => post.Parent)
                    .ThenInclude(parent => parent!.SlugNavigation)
                .SingleOrDefault();

            if (databasePost == null)
            {
                return null;
            }

            return ConvertPostToPostPage(databasePost);
        }

        public PostPage ConvertPostToPostPage(Post post)
        {
            IEnumerable<KeyValuePair<string, string>> parents = GetParentsStartingFromParent(post.Parent);

            Page databasePage = post.SlugNavigation;

            PostPage postPage = new PostPage
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

            return postPage;
        }

        public CategoryPage? GetCategory(int id)
        {
            PostCategory? databaseCategory = context.PostCategories
                .Where(category => category.CategoryId == id)
                .Include(category => category.InverseParent)
                    .ThenInclude(childCategory => childCategory.SlugNavigation)
                .Include(category => category.Parent)
                .Include(category => category.SlugNavigation)
                .Include(category => category.Posts)
                    .ThenInclude(post => post.SlugNavigation)
                .SingleOrDefault();

            if (databaseCategory == null)
            {
                return null;
            }

            return ConvertCategoryToCategoryPage(databaseCategory);
        }

        public CategoryPage? GetCategory(string slug)
        {
            PostCategory? databaseCategory = context.PostCategories
                .Where(category => category.Slug == slug)
                .Include(category => category.InverseParent)
                    .ThenInclude(childCategory => childCategory.SlugNavigation)
                .Include(category => category.Parent)
                .Include(category => category.SlugNavigation)
                .Include(category => category.Posts)
                    .ThenInclude(post => post.SlugNavigation)
                .SingleOrDefault();

            if (databaseCategory == null)
            {
                return null;
            }

            return ConvertCategoryToCategoryPage(databaseCategory);
        }

        private CategoryPage ConvertCategoryToCategoryPage(PostCategory databaseCategory)
        {
            IEnumerable<KeyValuePair<string, string>> parents = GetParentsStartingFromParent(databaseCategory.Parent);

            Page databasePage = databaseCategory.SlugNavigation;

            CategoryPage categoryPage = new CategoryPage
            {
                Body = databasePage.Body,
                Description = databasePage.Description,
                Keywords = databasePage.Keywords,
                Name = databasePage.Name,
                ParentCategories = parents,
                ChildrenCategories = databaseCategory.InverseParent.Select(category => KeyValuePair.Create(category.Slug, category.SlugNavigation.Name)),
                Posts = databaseCategory.Posts.Where(post => post.IsDraft == false).Select(post => KeyValuePair.Create(post.Slug, post.SlugNavigation.Name)),
                Slug = databasePage.Slug,
                Title = databasePage.Title,
                UseDateCreatedForSorting = databaseCategory.UseDateCreatedForSorting
            };

            return categoryPage;
        }

        private List<KeyValuePair<string, string>> GetParentsStartingFromParent(PostCategory? databaseCategory)
        {
            if (databaseCategory == null)
            {
                return [KeyValuePair.Create("/", "Index")];
            }

            List<KeyValuePair<string, string>> categoryParents = [];
            PostCategory? currentParent = databaseCategory;

            while (currentParent is not null)
            {
                Page? parentPage = context.Pages.SingleOrDefault(page => page.Slug == currentParent.Slug);

                if (parentPage == null)
                {
                    break;
                }

                categoryParents.Add(KeyValuePair.Create(currentParent.Slug, parentPage.Name));

                currentParent = context.PostCategories.SingleOrDefault(category => category.CategoryId == currentParent.ParentId);
            }

            categoryParents.Reverse();

            return categoryParents;
        }
    }
}