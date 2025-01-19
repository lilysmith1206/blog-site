using LylinkBackend_DatabaseAccessLayer.BusinessModels;
using LylinkBackend_DatabaseAccessLayer.Services;
using LylinkDb_UnitTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LylinkBackend_DatabaseAccessLayer_UnitTests
{
    public class LylinkDb_SlugUnitTests
    {
        [Fact]
        public void GetPostSlugs_ReturnsAllPostSlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            ISlugRepository repository = new SlugRepository(context);

            IEnumerable<string> slugs = repository.GetPostSlugs();

            Assert.NotNull(slugs);

            Assert.Contains(UnitTestData.IndexPost1.Slug, slugs);
            Assert.Contains(UnitTestData.TechPost1.Slug, slugs);
            Assert.Contains(UnitTestData.TechPost2.Slug, slugs);
            Assert.Contains(UnitTestData.TechPost3.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsPost1.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsPost2.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsPost3.Slug, slugs);
        }

        [Fact]
        public void GetCategorySlugs_ReturnsAllCategorySlugs()
        {
            var context = LylinkDb_InMemoryDatabase.GetFullDataInMemoryDatabase();

            ISlugRepository repository = new SlugRepository(context);

            IEnumerable<string> slugs = repository.GetCategorySlugs();

            Assert.NotNull(slugs);

            Assert.Contains(UnitTestData.IndexCategory.Slug, slugs);
            Assert.Contains(UnitTestData.TechCategory.Slug, slugs);
            Assert.Contains(UnitTestData.MostRecentPostsCategory.Slug, slugs);
        }
    }
}
