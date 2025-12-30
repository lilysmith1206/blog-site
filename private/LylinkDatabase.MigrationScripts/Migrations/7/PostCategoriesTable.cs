using FluentMigrator.Builders.Alter;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;
using System.Data;

namespace LylinkDatabase.MigrationScripts.Migrations._7;

public static class PostCategoriesTable
{
    public static void CopyPageDataForPostCategoriesToPages(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            INSERT INTO {MigrationConstants.PagesTableName} (slug, title, name, keywords, description, body)
            SELECT slug, title, categoryName, keywords, description, body FROM {MigrationConstants.PostCategoriesTableName};
        """);
    }

    public static void CopyPostCategoriesPageDataFromPagesToPostCategories(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            UPDATE p
            SET
                p.title = pg.title,
                p.categoryName = pg.name,
                p.keywords = pg.keywords,
                p.description = pg.description,
                p.body = pg.body
            FROM {MigrationConstants.PostCategoriesTableName} p
            INNER JOIN {MigrationConstants.PagesTableName} pg
                ON pg.slug = p.slug;
        """);
    }

    public static void DropPageColumnsFromPostCategories(this IDeleteExpressionRoot delete)
    {
        delete.Column("title")
            .FromTable(MigrationConstants.PostCategoriesTableName);

        delete.Column("categoryName")
            .FromTable(MigrationConstants.PostCategoriesTableName);

        delete.Column("keywords")
            .FromTable(MigrationConstants.PostCategoriesTableName);

        delete.Column("description")
            .FromTable(MigrationConstants.PostCategoriesTableName);
        
        delete.Column("body")
            .FromTable(MigrationConstants.PostCategoriesTableName);
    }

    public static void CreateSlugForeignKeyOnPostCategoriesToPages(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.PostCategoriesTableName)
            .AlterColumn("slug")
                .AsFixedLengthString(size: 40)
                .NotNullable()
                .ForeignKey(MigrationConstants.PostCategoryPageSlugForeignKeyName, MigrationConstants.PagesTableName, "slug")
                    .OnDeleteOrUpdate(Rule.Cascade);
    }

    public static void DeleteSlugForeignKeyOnPostCategoriesToPages(this IDeleteExpressionRoot delete)
    {
        delete.ForeignKey(MigrationConstants.PostCategoryPageSlugForeignKeyName)
            .OnTable(MigrationConstants.PostCategoriesTableName);
    }

    public static void CreatePageColumnsOnPostCategories(this ICreateExpressionRoot create)
    {
        create.Column("title")
            .OnTable(MigrationConstants.PostCategoriesTableName)
            .AsFixedLengthString(size: 80)
            .NotNullable();

        create.Column("categoryName")
            .OnTable(MigrationConstants.PostCategoriesTableName)
            .AsFixedLengthString(size: 80)
            .NotNullable();

        create.Column("keywords")
            .OnTable(MigrationConstants.PostCategoriesTableName)
            .AsFixedLengthString(size: 160)
            .NotNullable();

        create.Column("description")
            .OnTable(MigrationConstants.PostCategoriesTableName)
            .AsFixedLengthString(size: 160)
            .NotNullable();

        create.Column("body")
            .OnTable(MigrationConstants.PostCategoriesTableName)
            .AsString(size: 60000)
            .NotNullable();
    }
}
