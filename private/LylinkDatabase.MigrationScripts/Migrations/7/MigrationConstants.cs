namespace LylinkDatabase.MigrationScripts.Migrations._7;

public static class MigrationConstants
{
    public const string PostsTableName = "posts";
    public const string PagesTableName = "pages";
    public const string PostCategoriesTableName = "post_categories";
    public const string PostPageSlugForeignKeyName = "fk_posts_pages";
    public const string PostCategoryPageSlugForeignKeyName = "fk_post_categories_pages";
}
