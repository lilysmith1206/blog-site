using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;

namespace LylinkDatabase.MigrationScripts.Migrations._6;

public static class PostsTable
{
    public static void CreateTempPostsWithIntegerId(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.TempPostsTableName)
            .WithColumn("id")
                .AsInt32()
                .NotNullable()
                .PrimaryKey()
                .Identity()
            .WithColumn("slug")
                .AsFixedLengthString(size: 40)
                .NotNullable()
                .Unique()
            .WithColumn("parent_id")
                .AsInt32()
                .Nullable()
                .ForeignKey("fk_parentId", MigrationConstants.PostCategoriesTableName, "categoryId")
            .WithColumn("title")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .WithColumn("date_modified")
                .AsDateTime()
                .NotNullable()
            .WithColumn("name")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .WithColumn("keywords")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .WithColumn("description")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .WithColumn("body")
                .AsString(size: 60000)
                .NotNullable()
            .WithColumn("date_created")
                .AsDateTime()
                .NotNullable()
            .WithColumn("is_draft")
                .AsBoolean()
                .NotNullable();
    }

    public static void CreateTempPostsWithSlugAsId(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.TempPostsTableName)
            .WithColumn("slug")
                .AsFixedLengthString(size: 40)
                .NotNullable()
                .PrimaryKey()
            .WithColumn("parentId")
                .AsInt32()
                .Nullable()
                .ForeignKey("fk_parentId", MigrationConstants.PostCategoriesTableName, "categoryId")
            .WithColumn("title")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .WithColumn("date_modified")
                .AsDateTime()
                .NotNullable()
            .WithColumn("name")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .WithColumn("keywords")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .WithColumn("description")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .WithColumn("body")
                .AsString(size: 60000)
                .NotNullable()
            .WithColumn("date_created")
                .AsDateTime()
                .NotNullable()
            .WithColumn("is_draft")
                .AsBoolean()
                .NotNullable();
    }

    public static void CopyExistingPostDataToTempTable(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            INSERT INTO {MigrationConstants.TempPostsTableName} (slug, title, parent_id, date_modified, name, keywords, description, body, date_created, is_draft)
            SELECT slug, title, parentId, date_modified, name, keywords, description, body, date_created, is_draft FROM {MigrationConstants.PostsTableName};
        """);
    }

    public static void DeletePostsTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(MigrationConstants.PostsTableName);
    }

    public static void DeleteTempPostsTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(MigrationConstants.TempPostsTableName);
    }

    public static void RenameTempPostsTableToPostsTable(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            RENAME TABLE {MigrationConstants.TempPostsTableName} TO {MigrationConstants.PostsTableName};
        """);
    }
}
