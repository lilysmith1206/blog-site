using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;

namespace LylinkDatabase.MigrationScripts.Migrations._1;

public static class PostHierarchyTable
{
    public static void DeleteCategoryParentForeignKey(this IDeleteExpressionRoot delete)
    {
        delete.ForeignKey("fk_parentId")
                .OnTable(MigrationConstants.PostHierarchyTableName);
    }

    public static void CreateTemporaryPostHierarchyTableWithIntegerIds(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.TempPostHierarchyTableName)
            .WithColumn("categoryId")
                .AsInt32().PrimaryKey()
                .Identity()
            .WithColumn("parentId")
                .AsInt32()
                .Nullable()
            .WithColumn("categoryName")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("slug")
                .AsFixedLengthString(40)
                .Nullable()
            .WithColumn("title")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("keywords")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("description")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("body")
                .AsString(60000)
                .Nullable()
            .WithColumn("use_date_created_for_sorting")
                .AsBoolean()
                .Nullable();
    }

    public static void CreateTemporaryPostHierarchyTableWithGuidIds(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.TempPostHierarchyTableName)
            .WithColumn("categoryId")
                .AsString(size: 40)
                .NotNullable()
                .PrimaryKey()
            .WithColumn("parentId")
                .AsString(size: 40)
                .Nullable()
            .WithColumn("categoryName")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("slug")
                .AsFixedLengthString(40)
                .Nullable()
            .WithColumn("title")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("keywords")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("description")
                .AsFixedLengthString(80)
                .Nullable()
            .WithColumn("body")
                .AsString(60000)
                .Nullable()
            .WithColumn("use_date_created_for_sorting")
                .AsBoolean()
                .Nullable();
    }

    public static void CopyExistingPostHierarchyDataToTempTable(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            INSERT INTO {MigrationConstants.TempPostHierarchyTableName}
                (slug, title, categoryName, keywords, description, body, use_date_created_for_sorting)
            SELECT
                slug, title, name, keywords, description, body, use_date_created_for_sorting
            FROM {MigrationConstants.PostHierarchyTableName};
        """);
    }

    public static void MapExistingCategoryParentIdsToTempCategoryParentIds(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            UPDATE {MigrationConstants.TempPostHierarchyTableName} temp_categories
            JOIN post_hierarchy categories ON temp_categories.slug = categories.slug
            SET temp_categories.parentId = (
                SELECT temp.categoryId
                FROM {MigrationConstants.TempPostHierarchyTableName} temp
                WHERE temp.slug = (
                    SELECT parent.slug
                    FROM {MigrationConstants.PostHierarchyTableName} parent
                    WHERE parent.categoryId = categories.parentId
                )
            );
        """);
    }

    public static void RenameTempPostHierarchyTableToPostHiearchyTable(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            RENAME TABLE {MigrationConstants.TempPostHierarchyTableName} TO {MigrationConstants.PostHierarchyTableName};
        """);

    }

    public static void CreateForeignKeyForCategoryParent(this ICreateExpressionRoot create)
    {
        create.ForeignKey("fk_post_hierarchy_parent")
            .FromTable(MigrationConstants.PostHierarchyTableName)
                .ForeignColumn("parentId")
            .ToTable(MigrationConstants.PostHierarchyTableName)
                .PrimaryColumn("categoryId")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }
}
