using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkDb_UnitTests;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_PostUnit0ests
    {
        [Theory]
        [MemberData(nameof(GetCategoryFromIdUnitTestData))]
        public void GetCategoryFromId_ReturnsCategoryPage(int categoryId, PostPage expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            PostPage? categoryPage = repository.GetPost(categoryId);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromIdUnitTestData()
        {
            yield return [DatabaseUnitTestData.IndexPost1.Id, UnitTestData.IndexPost1];
            yield return [DatabaseUnitTestData.TechPost1.Id, UnitTestData.TechPost1];
            yield return [DatabaseUnitTestData.TechPost2.Id, UnitTestData.TechPost2];
            yield return [DatabaseUnitTestData.TechPost3.Id, UnitTestData.TechPost3];
            yield return [DatabaseUnitTestData.MostRecentPostsPost1.Id, UnitTestData.MostRecentPostsPost1];
            yield return [DatabaseUnitTestData.MostRecentPostsPost2.Id, UnitTestData.MostRecentPostsPost2];
            yield return [DatabaseUnitTestData.MostRecentPostsPost3.Id, UnitTestData.MostRecentPostsPost3];
        }

        [Theory]
        [MemberData(nameof(GetCategoryFromSlugUnitTestData))]
        public void GetCategoryFromSlug_ReturnsCategoryPage(string slug, PostPage expectedCategory)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            PostPage? categoryPage = repository.GetPost(slug);

            Assert.NotNull(categoryPage);

            Assert.Equivalent(expectedCategory, categoryPage);
        }

        public static IEnumerable<object[]> GetCategoryFromSlugUnitTestData()
        {
            yield return [DatabaseUnitTestData.IndexPost1.Slug, UnitTestData.IndexPost1];
            yield return [DatabaseUnitTestData.TechPost1.Slug, UnitTestData.TechPost1];
            yield return [DatabaseUnitTestData.TechPost2.Slug, UnitTestData.TechPost2];
            yield return [DatabaseUnitTestData.TechPost3.Slug, UnitTestData.TechPost3];
            yield return [DatabaseUnitTestData.MostRecentPostsPost1.Slug, UnitTestData.MostRecentPostsPost1];
            yield return [DatabaseUnitTestData.MostRecentPostsPost2.Slug, UnitTestData.MostRecentPostsPost2];
            yield return [DatabaseUnitTestData.MostRecentPostsPost3.Slug, UnitTestData.MostRecentPostsPost3];
        }

        /// <summary>
        /// For this test to work, the DatabaseUnitTestData must have the most recent posts and most recent post category be given the highest date time values for date modified.
        /// If this is not the case, the test will fail.
        /// </summary>
        [Theory]
        [MemberData(nameof(GetMostRecentPostsUnitTestData))]
        public void GetMostRecentPosts_ReturnsMostRecentPosts(int count, params PostPage[] expectedPostPages)
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            IEnumerable<KeyValuePair<string, string>> recentlyUpdatedPages = repository.GetRecentlyUpdatedPostInfos(count);

            foreach ((PostPage expected, KeyValuePair<string, string> actual) in expectedPostPages.Zip(recentlyUpdatedPages))
            {
                Assert.Equal(expected.Slug, actual.Key);
                Assert.Equal(expected.Name, actual.Value);
            }
        }

        public static IEnumerable<object[]> GetMostRecentPostsUnitTestData()
        {
            yield return [1, UnitTestData.MostRecentPostsPost3];
            yield return [2, UnitTestData.MostRecentPostsPost3, UnitTestData.MostRecentPostsPost2];
            yield return [3, UnitTestData.MostRecentPostsPost3, UnitTestData.MostRecentPostsPost2, UnitTestData.MostRecentPostsPost1];
        }

        [Fact]
        public void CreatePost_CreatesPost()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            const string Slug = "new-post";
            const string Title = "new post title";
            const string Name = "new post name";
            const string Keywords = "new post keywords";
            const string Description = "new post description";
            const string Body = "new post body";
            const bool IsDraft = false;
            const int ParentId = 1;

            int newPostId = repository.CreatePost(Slug, Title, Name, Keywords, Description, Body, IsDraft, ParentId);

            PostPage? post = repository.GetPost(newPostId);

            Assert.NotNull(post);

            Assert.Equal(Slug, post.Value.Slug);
            Assert.Equal(Title, post.Value.Title);
            Assert.Equal(Name, post.Value.Name);
            Assert.Equal(Keywords, post.Value.Keywords);
            Assert.Equal(Description, post.Value.Description);
            Assert.Equal(Body, post.Value.Body);
            Assert.Equal(IsDraft, post.Value.IsDraft);
            Assert.Equal(ParentId, DatabaseUnitTestData.IndexCategory.CategoryId);
        }

        [Fact]
        public void CreatePost_GivenExistingPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreatePost(UnitTestData.IndexPost1.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void CreatePost_GivenExistingCategorySlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.CreatePost(UnitTestData.IndexCategory.Slug, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }

        [Fact]
        public void UpdatePost_UpdatesExistingPostPageData()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            int updatedPostId = repository.UpdatePost(
                UnitTestData.IndexPost1.Slug,
                UnitTestData.IndexPost1.Title + "A",
                UnitTestData.IndexPost1.Name + "A",
                UnitTestData.IndexPost1.Keywords + "A",
                UnitTestData.IndexPost1.Description + "A",
                UnitTestData.IndexPost1.Body + "A",
                UnitTestData.IndexPost1.IsDraft == false,
                DatabaseUnitTestData.IndexCategory.CategoryId);

            PostPage? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexPost1.Title, post.Value.Title);
            Assert.NotEqual(UnitTestData.IndexPost1.Name, post.Value.Name);
            Assert.NotEqual(UnitTestData.IndexPost1.Keywords, post.Value.Keywords);
            Assert.NotEqual(UnitTestData.IndexPost1.Description, post.Value.Description);
            Assert.NotEqual(UnitTestData.IndexPost1.Body, post.Value.Body);
            Assert.NotEqual(UnitTestData.IndexPost1.IsDraft, post.Value.IsDraft);
        }

        [Fact]
        public void UpdatePost_GivenNewCategoryId_UpdatesPostToBeUnderNewCategory()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            int updatedPostId = repository.UpdatePost(
                UnitTestData.IndexPost1.Slug,
                UnitTestData.IndexPost1.Title,
                UnitTestData.IndexPost1.Name,
                UnitTestData.IndexPost1.Keywords,
                UnitTestData.IndexPost1.Description,
                UnitTestData.IndexPost1.Body,
                UnitTestData.IndexPost1.IsDraft,
                DatabaseUnitTestData.TechCategory.CategoryId);

            PostPage? post = repository.GetPost(updatedPostId);

            Assert.NotNull(post);

            Assert.NotEqual(UnitTestData.IndexPost1.Parents.First().Key, post.Value.Parents.Last().Key);
            Assert.NotEqual(UnitTestData.IndexPost1.Parents.First().Value, post.Value.Parents.Last().Value);
        }

        [Fact]
        public void UpdatePost_GivenNonexistentPostSlug_ThrowsInvalidOperationException()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            IPageRepository repository = new PageRepository(context);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _ = repository.UpdatePost("nonexistent post slug", string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, false, null);
            });
        }
    }
}
