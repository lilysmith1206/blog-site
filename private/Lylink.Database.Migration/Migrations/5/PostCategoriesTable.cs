using FluentMigrator.Builders.Alter;

namespace Lylink.Database.Migration._5;

public static class PostCategoriesTable
{
    public static void AddNullableOptionsOnPostCategories(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.PostCategoriesTableName)
            .AlterColumn("categoryName")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .AlterColumn("slug")
                .AsFixedLengthString(size: 40)
                .Nullable()
            .AlterColumn("title")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .AlterColumn("keywords")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .AlterColumn("description")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .AlterColumn("body")
                .AsString(60000)
                .Nullable()
            .AlterColumn("use_date_created_for_sorting")
                .AsBoolean()
                .Nullable();
    }

    public static void RemoveNullableOptionsFromPostCategories(this IAlterExpressionRoot alter)
    {
        /*ALTER TABLE post_categories MODIFY parentId INT NULL;
ALTER TABLE post_categories MODIFY categoryName CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY slug CHAR(40) NOT NULL;
ALTER TABLE post_categories MODIFY title CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY keywords CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY description CHAR(80) NOT NULL;
ALTER TABLE post_categories MODIFY body VARCHAR(60000) NOT NULL;
UPDATE post_categories SET use_date_created_for_sorting = 0 WHERE use_date_created_for_sorting IS NULL;
ALTER TABLE post_categories MODIFY use_date_created_for_sorting TINYINT(1) NOT NULL;*/
        alter.Table(MigrationConstants.PostCategoriesTableName)
            .AlterColumn("categoryName")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .AlterColumn("slug")
                .AsFixedLengthString(size: 40)
                .NotNullable()
            .AlterColumn("title")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .AlterColumn("keywords")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .AlterColumn("description")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .AlterColumn("body")
                .AsString(60000)
                .NotNullable()
            .AlterColumn("use_date_created_for_sorting")
                .AsBoolean()
                .NotNullable();
    }
}
