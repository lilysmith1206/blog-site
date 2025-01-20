using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkDb_UnitTests;

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
            yield return [DatabaseUnitTestData.IndexCategory.CategoryId, UnitTestData.IndexCategoryInfo];
            yield return [DatabaseUnitTestData.TechCategory.CategoryId, UnitTestData.TechCategoryInfo];
            yield return [DatabaseUnitTestData.MostRecentPostsCategory.CategoryId, UnitTestData.MostRecentPostsCategoryInfo];
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
            const bool IsSortedByDateCreated = false;
            int parentId = UnitTestData.IndexCategoryInfo.Id;

            CategoryInfo newCategoryInfo = new CategoryInfo
            {
                Body = Body,
                Description = Description,
                UseDateCreatedForSorting = IsSortedByDateCreated,
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
            Assert.Equal(IsSortedByDateCreated, post.UseDateCreatedForSorting);
            Assert.Equal(parentId, UnitTestData.IndexCategoryInfo.Id);
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
                    Slug = UnitTestData.IndexPostPage1.Slug,
                    Body = string.Empty,
                    Description = string.Empty,
                    Keywords = string.Empty,
                    Name = string.Empty,
                    Title = string.Empty,
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
                    Slug = UnitTestData.IndexPostPage1.Slug,
                    Body = string.Empty,
                    Description = string.Empty,
                    Keywords = string.Empty,
                    Name = string.Empty,
                    Title = string.Empty,
                });
            });
        }

        [Fact]
        public void UpdateCategory_UpdatesExistingCategoryPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            CategoryInfo categoryInfo = new CategoryInfo
            {
                Slug = UnitTestData.IndexCategoryInfo.Slug,
                Title = UnitTestData.IndexCategoryInfo.Title + "A",
                Name = UnitTestData.IndexCategoryInfo.Name + "A",
                Keywords = UnitTestData.IndexCategoryInfo.Keywords + "A",
                Description = UnitTestData.IndexCategoryInfo.Description + "A",
                Body = UnitTestData.IndexCategoryInfo.Body + "A",
                UseDateCreatedForSorting = UnitTestData.IndexCategoryInfo.UseDateCreatedForSorting == false,
                ParentId = null
            };

            int updatedCategoryId = repository.UpdateCategory(categoryInfo);

            CategoryInfo? post = repository.GetCategory(updatedCategoryId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Title, post.Title);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Name, post.Name);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Keywords, post.Keywords);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Description, post.Description);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.Body, post.Body);
            Assert.NotEqual(UnitTestData.IndexCategoryInfo.UseDateCreatedForSorting, post.UseDateCreatedForSorting);
        }

        [Fact]
        public void UpdateCategory_GivenNewCategoryId_UpdatesCategoryToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageManagementRepository repository = new PageManagementRepository(context);

            CategoryInfo categoryInfo = new CategoryInfo
            {
                Slug = UnitTestData.MostRecentPostsCategoryInfo.Slug,
                Title = UnitTestData.MostRecentPostsCategoryInfo.Title,
                Name = UnitTestData.MostRecentPostsCategoryInfo.Name,
                Keywords = UnitTestData.MostRecentPostsCategoryInfo.Keywords,
                Description = UnitTestData.MostRecentPostsCategoryInfo.Description,
                Body = UnitTestData.MostRecentPostsCategoryInfo.Body,
                UseDateCreatedForSorting = UnitTestData.MostRecentPostsCategoryInfo.UseDateCreatedForSorting,
                ParentId = DatabaseUnitTestData.TechCategory.CategoryId
            };

            int updatedPostId = repository.UpdateCategory(categoryInfo);
            CategoryInfo ? category = repository.GetCategory(updatedPostId);

            Assert.NotNull(category);

            Assert.Equal(UnitTestData.TechCategoryInfo.Id, category.ParentId);
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
                });
            });
        }
    }
}
