using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;

namespace LylinkDb_UnitTests
{
    public class LylinkDb_PostCategoryUnitTests
    {
        [Theory]
        [MemberData(nameof(GetCategoryFromIdUnitTestData))]
        public void GetCategoryFromId_ReturnsCategoryPage(int categoryId, CategoryPage expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            CategoryPage? categoryPage = repository.GetCategory(categoryId);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromIdUnitTestData()
        {
            yield return [DatabaseUnitTestData.IndexCategory.CategoryId, UnitTestData.IndexCategory];
            yield return [DatabaseUnitTestData.TechCategory.CategoryId, UnitTestData.TechCategory];
            yield return [DatabaseUnitTestData.MostRecentPostsCategory.CategoryId, UnitTestData.MostRecentPostsCategory];
        }

        [Theory]
        [MemberData(nameof(GetCategoryFromSlugUnitTestData))]
        public void GetCategoryFromSlug_ReturnsCategoryPage(string slug, CategoryPage expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            CategoryPage? categoryPage = repository.GetCategory(slug);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromSlugUnitTestData()
        {
            yield return [DatabaseUnitTestData.IndexCategory.Slug, UnitTestData.IndexCategory];
            yield return [DatabaseUnitTestData.TechCategory.Slug, UnitTestData.TechCategory];
            yield return [DatabaseUnitTestData.MostRecentPostsCategory.Slug, UnitTestData.MostRecentPostsCategory];
        }

        [Fact]
        public void CreateCategory_CreatesCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            const string Slug = "new-post";
            const string Title = "new post title";
            const string Name = "new post name";
            const string Keywords = "new post keywords";
            const string Description = "new post description";
            const string Body = "new post body";
            const bool isSortedByDateCreated = false;
            const int ParentId = 1;

            int newPostId = repository.CreateCategory(Slug, Title, Name, Keywords, Description, Body, isSortedByDateCreated, ParentId);

            CategoryPage? post = repository.GetCategory(newPostId);

            Assert.NotNull(post);

            Assert.Equal(Slug, post.Value.Slug);
            Assert.Equal(Title, post.Value.Title);
            Assert.Equal(Name, post.Value.Name);
            Assert.Equal(Keywords, post.Value.Keywords);
            Assert.Equal(Description, post.Value.Description);
            Assert.Equal(Body, post.Value.Body);
            Assert.Equal(isSortedByDateCreated, post.Value.UseDateCreatedForSorting);
            Assert.Equal(ParentId, DatabaseUnitTestData.IndexCategory.CategoryId);
        }

        [Fact]
        public void CreateCategory_GivenExistingPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreateCategory(UnitTestData.IndexPost1.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void CreateCategory_GivenExistingCategorySlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreateCategory(UnitTestData.IndexCategory.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void UpdateCategory_UpdatesExistingCategoryPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            int updatedCategoryId = repository.UpdateCategory(
                UnitTestData.IndexCategory.Slug,
                UnitTestData.IndexCategory.Title + "A",
                UnitTestData.IndexCategory.Name + "A",
                UnitTestData.IndexCategory.Keywords + "A",
                UnitTestData.IndexCategory.Description + "A",
                UnitTestData.IndexCategory.Body + "A",
                UnitTestData.IndexCategory.UseDateCreatedForSorting == false,
                null);

            CategoryPage? post = repository.GetCategory(updatedCategoryId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexCategory.Title, post.Value.Title);
            Assert.NotEqual(UnitTestData.IndexCategory.Name, post.Value.Name);
            Assert.NotEqual(UnitTestData.IndexCategory.Keywords, post.Value.Keywords);
            Assert.NotEqual(UnitTestData.IndexCategory.Description, post.Value.Description);
            Assert.NotEqual(UnitTestData.IndexCategory.Body, post.Value.Body);
            Assert.NotEqual(UnitTestData.IndexCategory.UseDateCreatedForSorting, post.Value.UseDateCreatedForSorting);
        }

        [Fact]
        public void UpdateCategory_GivenNewCategoryId_UpdatesCategoryToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            int updatedPostId = repository.UpdateCategory(
                UnitTestData.MostRecentPostsCategory.Slug,
                UnitTestData.MostRecentPostsCategory.Title,
                UnitTestData.MostRecentPostsCategory.Name,
                UnitTestData.MostRecentPostsCategory.Keywords,
                UnitTestData.MostRecentPostsCategory.Description,
                UnitTestData.MostRecentPostsCategory.Body,
                UnitTestData.MostRecentPostsCategory.UseDateCreatedForSorting,
                DatabaseUnitTestData.TechCategory.CategoryId);

            CategoryPage? category = repository.GetCategory(updatedPostId);

            Assert.NotNull(category);

            Assert.Equal(DatabaseUnitTestData.TechCategory.Slug, category.Value.ParentCategories.First().Key);
            Assert.Equal(DatabaseUnitTestData.TechCategory.SlugNavigation.Name, category.Value.ParentCategories.First().Value);
        }

        [Fact]
        public void UpdatePost_GivenNonexistentPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.UpdateCategory("nonexistent post slug", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        /*
        [Fact]
        public void GetAllCategorySlugs_ShouldReturnCategorySlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostCategoryDatabaseService service = new PageDatabaseService(context);

            var result = service.GetAllCategorySlugs();

            Assert.Equal(UnitTestData.Categories.Count, result.Count());

            Assert.Contains(UnitTestData.TechCategory.Slug, result);
            Assert.Contains(UnitTestData.BlogCategory.Slug, result);
            Assert.Contains(UnitTestData.IndexCategory.Slug, result);
            Assert.Contains(UnitTestData.WritingCategory.Slug, result);
            Assert.Contains(UnitTestData.BrushCategory.Slug, result);
        }

        [Fact]
        public void GetChildCategoriesOfCategory_ShouldReturnChildCategories()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostCategoryDatabaseService service = new PageDatabaseService(context);

            var result = service.GetChildCategoriesOfCategory(UnitTestData.BlogCategory.CategoryId);

            Assert.NotNull(result);
            Assert.Single(result);

            var brushCategory = result.Single();

            Assert.Equal(UnitTestData.BrushCategory.CategoryId, brushCategory.CategoryId);
            Assert.Equal(UnitTestData.BrushCategory.Slug, brushCategory.Slug);
            Assert.Equal(UnitTestData.BrushCategory.ParentId, brushCategory.ParentId);
            Assert.Equal(UnitTestData.BrushCategory.UseDateCreatedForSorting, brushCategory.UseDateCreatedForSorting);
            Assert.Equal(UnitTestData.BrushCategory.Title, brushCategory.Title);
        }

        [Theory]
        [ClassData(typeof(PostCategoryParentCategoriesTestDataProvider))]
        public void GetParentCategoriesFromCategoryId_ReturnsOnlyIndexWhenThatIsOnlyParent(int categoryId, params PostCategory[] expectedResults)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPostCategoryDatabaseService service = new PageDatabaseService(context);

            var result = service.GetParentCategoriesFromCategoryId(categoryId);

            foreach ((var expectedCategory, var actualCategory) in expectedResults.Zip(result))
            {
                Assert.Equal(expectedCategory, actualCategory);
            }
        }
        */
    }

    /*
    public class PostCategoryParentCategoriesTestDataProvider : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { UnitTestData.IndexCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.BlogCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.WritingCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.TechCategory.CategoryId, UnitTestData.IndexCategory };
            yield return new object[] { UnitTestData.BrushCategory.CategoryId, UnitTestData.BlogCategory, UnitTestData.IndexCategory };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    */
}