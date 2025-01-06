﻿using LylinkBackend_DatabaseAccessLayer.Models;

namespace LylinkBackend_DatabaseAccessLayer.Services
{
    public interface IPostCategoryDatabaseService
    {
        public IEnumerable<string?> GetAllCategorySlugs();

        public PostCategory? GetCategoryFromSlug(string slug);

        public IEnumerable<PostCategory> GetAllCategories();

        public PostCategory? GetCategoryFromId(int categoryId);

        public List<PostCategory> GetParentCategories(int? categoryId);

        public List<PostCategory> GetChildCategoriesOfCategory(int categoryId);

        public bool UpdateCategory(PostCategory category);

        public bool CreateCategory(PostCategory category);
    }
}
