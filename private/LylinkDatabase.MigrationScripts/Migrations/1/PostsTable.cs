using FluentMigrator;
using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;

namespace LylinkDatabase.MigrationScripts.Migrations._1;

public static class PostsTable
{
    public static void DeletePostParentForeignKey(this IDeleteExpressionRoot delete)
    {
        delete.ForeignKey("fk_parentId")
                .OnTable(MigrationConstants.PostsTableName);
    }

    public static void CreatePostParentForeignKey(this ICreateExpressionRoot create)
    {
        create
            .ForeignKey("fk_parentId")
            .FromTable(MigrationConstants.PostsTableName)
                .ForeignColumn("parentId")
            .ToTable(MigrationConstants.PostHierarchyTableName)
                .PrimaryColumn("categoryId")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public static void CreateTemporaryPostsTableWithIntegerIds(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.TempPostsTableName)
            .WithColumn("slug")
                .AsFixedLengthString(40)
                .PrimaryKey()
            .WithColumn("title")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("parentId")
                .AsInt32()
                .Nullable()
            .WithColumn("date_modified")
                .AsDateTime()
                .WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("name")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("keywords")
                .AsFixedLengthString(160)
                .Nullable()
            .WithColumn("description")
                .AsFixedLengthString(160)
                .Nullable()
            .WithColumn("body")
                .AsString(60000)
                .Nullable()
            .WithColumn("date_created")
                .AsDateTime()
                .WithDefault(SystemMethods.CurrentDateTime);
    }

    public static void CreateTemporaryPostsTableWithGuidIds(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.TempPostsTableName)
            .WithColumn("slug")
                .AsFixedLengthString(40)
                .PrimaryKey()
            .WithColumn("title")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("parentId")
                .AsFixedLengthString(size: 40)
                .Nullable()
            .WithColumn("date_modified")
                .AsDateTime()
                .WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("name")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("keywords")
                .AsFixedLengthString(160)
                .Nullable()
            .WithColumn("description")
                .AsFixedLengthString(160)
                .Nullable()
            .WithColumn("body")
                .AsString(60000)
                .Nullable()
            .WithColumn("date_created")
                .AsDateTime()
                .WithDefault(SystemMethods.CurrentDateTime);
    }

    public static void CopyExistingPostsDataToTempTable(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            INSERT INTO {MigrationConstants.TempPostsTableName}
                (slug, title, date_modified, name, keywords, description, body, date_created)
            SELECT
                slug, title, date_modified, name, keywords, description, body, date_created
            FROM {MigrationConstants.PostsTableName};
        """);
    }

    public static void MapExistingPostParentsToTempHierarchyParents(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            UPDATE {MigrationConstants.TempPostsTableName} t
            JOIN {MigrationConstants.PostsTableName} o ON t.slug = o.slug
            SET t.parentId = (
                SELECT temp.categoryId
                FROM {MigrationConstants.TempPostHierarchyTableName} temp
                WHERE temp.slug = (
                    SELECT parent.slug
                    FROM {MigrationConstants.PostHierarchyTableName} parent
                    WHERE parent.categoryId = o.parentId
                )
            );
        """);
    }

    public static void RenameTempPostsToPosts(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            RENAME TABLE {MigrationConstants.TempPostsTableName} TO {MigrationConstants.PostsTableName};
        """);
    }

    public static void CreateForeignKeyForPostParent(this ICreateExpressionRoot create)
    {
        create.ForeignKey("fk_posts_parent")
            .FromTable(MigrationConstants.PostsTableName)
                .ForeignColumn("parentId")
            .ToTable(MigrationConstants.PostHierarchyTableName)
                .PrimaryColumn("categoryId")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}
