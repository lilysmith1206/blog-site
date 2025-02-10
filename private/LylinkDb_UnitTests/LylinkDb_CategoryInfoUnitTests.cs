using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Mappers;
using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_CategoryInfoUnitTests
    {
        [Theory]
        [MemberData(nameof(GetCategoryFromIdUnitTestData))]
        public void GetCategoryFromId_ReturnsCategoryPage(int categoryId, CategoryInfo expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            CategoryInfo? categoryPage = repository.GetCategory(categoryId);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromIdUnitTestData()
        {
            DatabaseUnitTestData.IndexCategory.Map((PostSortingMethod)(DatabaseUnitTestData.IndexCategory.PostSortingMethodId ?? 1), out CategoryInfo indexCategoryInfo);
            DatabaseUnitTestData.TechCategory.Map((PostSortingMethod)(DatabaseUnitTestData.TechCategory.PostSortingMethodId ?? 1), out CategoryInfo techCategoryInfo);
            DatabaseUnitTestData.MostRecentPostsCategory.Map((PostSortingMethod)(DatabaseUnitTestData.MostRecentPostsCategory.PostSortingMethodId ?? 1), out CategoryInfo mostRecentPostsCategoryInfo);

            yield return [DatabaseUnitTestData.IndexCategory.CategoryId, indexCategoryInfo];
            yield return [DatabaseUnitTestData.TechCategory.CategoryId, techCategoryInfo];
            yield return [DatabaseUnitTestData.MostRecentPostsCategory.CategoryId, mostRecentPostsCategoryInfo];
        }

        [Fact]
        public void CreateCategory_CreatesCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            const string Slug = "new-post";
            const string Title = "new post title";
            const string Name = "new post name";
            const string Keywords = "new post keywords";
            const string Description = "new post description";
            const string Body = "new post body";
            const PostSortingMethod PostSortingMethod = PostSortingMethod.ByDateCreatedAscending;
            int parentId = DatabaseUnitTestData.IndexCategory.CategoryId;

            CategoryInfo newCategoryInfo = new CategoryInfo
            {
                Body = Body,
                Description = Description,
                PostSortingMethod = PostSortingMethod,
                Keywords = Keywords,
                Name = Name,
                ParentId = parentId,
                Slug = Slug,
                Title = Title
            };

            int newPostId = repository.CreateCategory(newCategoryInfo);

            CategoryInfo? post = repository.GetCategory(newPostId);

            Assert.NotNull(post);

            Assert.Equal(Slug, post.Slug);
            Assert.Equal(Title, post.Title);
            Assert.Equal(Name, post.Name);
            Assert.Equal(Keywords, post.Keywords);
            Assert.Equal(Description, post.Description);
            Assert.Equal(Body, post.Body);
            Assert.Equal(PostSortingMethod, post.PostSortingMethod);
            Assert.Equal(parentId, DatabaseUnitTestData.IndexCategory.CategoryId);
        }

        [Fact]
        public void CreateCategory_GivenExistingPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreateCategory(new CategoryInfo
                {
                    Slug = DatabaseUnitTestData.IndexPost1.SlugNavigation.Slug,
                    Body = string.Empty,
                    Description = string.Empty,
                    Keywords = string.Empty,
                    Name = string.Empty,
                    Title = string.Empty,
                    PostSortingMethod = PostSortingMethod.ByDateCreatedAscending
                });
            });
        }

        [Fact]
        public void CreateCategory_GivenExistingCategorySlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreateCategory(new CategoryInfo
                {
                    Slug = DatabaseUnitTestData.IndexPost1.SlugNavigation.Slug,
                    Body = string.Empty,
                    Description = string.Empty,
                    Keywords = string.Empty,
                    Name = string.Empty,
                    Title = string.Empty,
                    PostSortingMethod = PostSortingMethod.ByDateCreatedAscending
                });
            });
        }

        [Fact]
        public void UpdateCategory_UpdatesExistingCategoryPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            string slug = DatabaseUnitTestData.IndexCategory.Slug;
            string title = DatabaseUnitTestData.IndexCategory.SlugNavigation.Title;
            string name = DatabaseUnitTestData.IndexCategory.SlugNavigation.Name;
            string keywords = DatabaseUnitTestData.IndexCategory.SlugNavigation.Keywords;
            string description = DatabaseUnitTestData.IndexCategory.SlugNavigation.Description;
            string body = DatabaseUnitTestData.IndexCategory.SlugNavigation.Body;
            
            _ = Enum.TryParse(DatabaseUnitTestData.IndexCategory.PostSortingMethod?.SortingName, out PostSortingMethod sortingMethod);
            
            CategoryInfo categoryInfo = new CategoryInfo
            {
                Slug = slug,
                Title = title + "A",
                Name = name + "A",
                Keywords = keywords + "A",
                Description = description + "A",
                Body = body + "A",
                PostSortingMethod = PostSortingMethod.ByDateModifiedAscending,
                ParentId = null
            };

            int updatedCategoryId = repository.UpdateCategory(categoryInfo);

            CategoryInfo? post = repository.GetCategory(updatedCategoryId);

            Assert.NotNull(post);
                                                                        
            Assert.NotEqual(title, post.Title);
            Assert.NotEqual(name, post.Name);
            Assert.NotEqual(keywords, post.Keywords);
            Assert.NotEqual(description, post.Description);
            Assert.NotEqual(body, post.Body);
            Assert.NotEqual(sortingMethod, post.PostSortingMethod);
        }

        [Fact]
        public void UpdateCategory_GivenNewCategoryId_UpdatesCategoryToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            string slug = DatabaseUnitTestData.IndexCategory.Slug;
            string title = DatabaseUnitTestData.IndexCategory.SlugNavigation.Title;
            string name = DatabaseUnitTestData.IndexCategory.SlugNavigation.Name;
            string keywords = DatabaseUnitTestData.IndexCategory.SlugNavigation.Keywords;
            string description = DatabaseUnitTestData.IndexCategory.SlugNavigation.Description;
            string body = DatabaseUnitTestData.IndexCategory.SlugNavigation.Body;
            const PostSortingMethod CategorySortingMethod = PostSortingMethod.ByDateModifiedDescending;

            CategoryInfo categoryInfo = new CategoryInfo
            {
                Slug = slug,
                Title = title,
                Name = name,
                Keywords = keywords,
                Description = description,
                Body = body,
                PostSortingMethod = CategorySortingMethod,
                ParentId = DatabaseUnitTestData.TechCategory.CategoryId
            };

            int updatedPostId = repository.UpdateCategory(categoryInfo);
            CategoryInfo? category = repository.GetCategory(updatedPostId);

            Assert.NotNull(category);

            Assert.Equal(DatabaseUnitTestData.TechCategory.CategoryId, category.ParentId);
        }

        [Fact]
        public void UpdatePost_GivenNonexistentPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.UpdateCategory(new CategoryInfo
                {
                    Slug = "Non existent slug",
                    Body = string.Empty,
                    Description = string.Empty,
                    Keywords = string.Empty,
                    Name = string.Empty,
                    Title = string.Empty,
                    PostSortingMethod = PostSortingMethod.ByDateCreatedAscending
                });
            });
        }
    }
}
