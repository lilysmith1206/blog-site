using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Mappers;
using LylinkBackend_DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public class PageRepository(LylinkdbContext context) : IPageRepository
    {
        private readonly PostCategory IndexCategory = context.PostCategories.Single(category => category.Slug == "/");

        public IEnumerable<PageLink> GetRecentlyUpdatedPostInfos(int amount)
        {
            return context.Posts
                .OrderByDescending(post => post.DateModified)
                .Where(post => post.IsDraft == false)
                .Take(amount)
                .Include(post => post.SlugNavigation)
                .Select(post => new PageLink { Description = post.SlugNavigation.Description, Name = post.SlugNavigation.Name, Slug = post.Slug });
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

            IEnumerable<PageLink> parents = GetParentCategories(databasePost.Parent);

            databasePost.Map(parents, out PostPage page);

            return page;
        }

        public CategoryPage? GetCategory(string slug)
        {
            PostCategory? databaseCategory = context.PostCategories
                .Where(category => category.Slug == slug)
                .Include(category => category.InverseParent)
                    .ThenInclude(childCategory => childCategory.SlugNavigation)
                .Include(category => category.Parent)
                .Include(category => category.SlugNavigation)
                .Include(category => category.Posts.Where(post => post.IsDraft == false))
                    .ThenInclude(post => post.SlugNavigation)
                .Include(category => category.PostSortingMethod)
                .SingleOrDefault();

            if (databaseCategory == null)
            {
                return null;
            }

            IEnumerable<PageLink> parents = GetParentCategories(databaseCategory.Parent);

            BusinessModels.PostSortingMethod postSortingMethod = databaseCategory.PostSortingMethod!.Map();

            IEnumerable<PageLink> postLinks = (postSortingMethod switch
            {
                BusinessModels.PostSortingMethod.ByDateCreatedAscending => databaseCategory.Posts.OrderBy(post => post.DateCreated),
                BusinessModels.PostSortingMethod.ByDateCreatedDescending => databaseCategory.Posts.OrderByDescending(post => post.DateCreated),
                BusinessModels.PostSortingMethod.ByDateModifiedAscending => databaseCategory.Posts.OrderBy(post => post.DateModified),
                BusinessModels.PostSortingMethod.ByDateModifiedDescending => databaseCategory.Posts.OrderByDescending(post => post.DateModified),
                _ => throw new NotSupportedException($"Sorting method")
            }).Select(post => post.Map());

            IEnumerable<PageLink> childCategories = databaseCategory.InverseParent.Select(childCategory => childCategory.Map());

            databaseCategory.Map(postLinks, parents, childCategories, postSortingMethod, out CategoryPage page);

            return page;
        }

        private IEnumerable<PageLink> GetParentCategories(PostCategory? parentCategory)
        {
            IEnumerable<PageLink> parents;

            if (parentCategory is null)
            {
                PostCategory? indexCategory = context.PostCategories.Include(category => category.SlugNavigation)
                    .SingleOrDefault(category => category.Slug == "/");

                if (indexCategory == null)
                {
                    throw new NullReferenceException("Index category with slug '/' does not exist in the database!");
                }

                parents = [new PageLink { Description = indexCategory.SlugNavigation.Description, Name = indexCategory.SlugNavigation.Name, Slug = indexCategory.Slug }];
            }
            else
            {
                parents = MapToParentList(parentCategory, context.PostCategories.Include(category => category.SlugNavigation));
            }

            return parents;
        }

        public static List<PageLink> MapToParentList(PostCategory databaseCategory, IEnumerable<PostCategory> possibleParentCategories)
        {
            List<PageLink> categoryParents = [];
            PostCategory? currentParent = databaseCategory;

            while (currentParent is not null)
            {
                PostCategory? parentPage = possibleParentCategories.SingleOrDefault(category => category.CategoryId == currentParent.CategoryId);

                if (parentPage == null)
                {
                    break;
                }

                categoryParents.Add(currentParent.Map());

                currentParent = possibleParentCategories.SingleOrDefault(category => category.CategoryId == currentParent.ParentId);
            }

            categoryParents.Reverse();

            return categoryParents;
        }
    }
}