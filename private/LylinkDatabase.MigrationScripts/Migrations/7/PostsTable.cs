using FluentMigrator.Builders.Alter;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;
using System.Data;

namespace LylinkDatabase.MigrationScripts.Migrations._7;

public static class PostsTable
{
    public static void CopyPageDataForPostsToPages(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            INSERT INTO {MigrationConstants.PagesTableName} (slug, title, name, keywords, description, body)
            SELECT slug, title, name, keywords, description, body FROM {MigrationConstants.PostsTableName};
        """);
    }

    public static void CopyPostsPageDataFromPagesToPosts(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            UPDATE p
            SET
                p.title = pg.title,
                p.name = pg.name,
                p.keywords = pg.keywords,
                p.description = pg.description,
                p.body = pg.body
            FROM {MigrationConstants.PostsTableName} p
            INNER JOIN {MigrationConstants.PagesTableName} pg
                ON pg.slug = p.slug;
        """);
    }

    public static void DropPageColumnsFromPosts(this IDeleteExpressionRoot delete)
    {
        delete.Column("title")
            .FromTable(MigrationConstants.PostsTableName);

        delete.Column("name")
            .FromTable(MigrationConstants.PostsTableName);

        delete.Column("keywords")
            .FromTable(MigrationConstants.PostsTableName);

        delete.Column("description")
            .FromTable(MigrationConstants.PostsTableName);
        
        delete.Column("body")
            .FromTable(MigrationConstants.PostsTableName);
    }

    public static void CreateSlugForeignKeyOnPostsToPages(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.PostsTableName)
            .AlterColumn("slug")
                .AsFixedLengthString(size: 40)
                .NotNullable()
                .ForeignKey(MigrationConstants.PostPageSlugForeignKeyName, MigrationConstants.PagesTableName, "slug")
                    .OnDeleteOrUpdate(Rule.Cascade);
    }

    public static void DeleteSlugForeignKeyOnPostsToPages(this IDeleteExpressionRoot delete)
    {
        delete.ForeignKey(MigrationConstants.PostPageSlugForeignKeyName)
            .OnTable(MigrationConstants.PostsTableName);
    }

    public static void CreatePageColumnsOnPost(this ICreateExpressionRoot create)
    {
        create.Column("title")
            .OnTable(MigrationConstants.PostsTableName)
            .AsFixedLengthString(size: 80)
            .NotNullable();

        create.Column("name")
            .OnTable(MigrationConstants.PostsTableName)
            .AsFixedLengthString(size: 80)
            .NotNullable();

        create.Column("keywords")
            .OnTable(MigrationConstants.PostsTableName)
            .AsFixedLengthString(size: 160)
            .NotNullable();

        create.Column("description")
            .OnTable(MigrationConstants.PostsTableName)
            .AsFixedLengthString(size: 160)
            .NotNullable();

        create.Column("body")
            .OnTable(MigrationConstants.PostsTableName)
            .AsString(size: 60000)
            .NotNullable();
    }
}
