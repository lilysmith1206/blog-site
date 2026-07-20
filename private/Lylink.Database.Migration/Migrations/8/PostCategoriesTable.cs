using FluentMigrator.Builders.Alter;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;
using System.Data;

namespace Lylink.Database.Migration._8;

public static class PostCategoriesTable
{
    public static void AddPostSortingMethodColumn(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.PostCategoriesTableName)
            .AddColumn("post_sorting_method_id")
            .AsInt32()
            .Nullable()
            .ForeignKey(MigrationConstants.CategorySortingMethodForeignKeyName, MigrationConstants.PostSortingMethodsTableName, "id")
                .OnDeleteOrUpdate(Rule.Cascade);
    }

    public static void PopulatePostSortingMethodIds(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            UPDATE {MigrationConstants.PostCategoriesTableName}
            SET post_sorting_method_id = 2
            WHERE use_date_created_for_sorting = 1;
        """);

        execute.Sql($"""
            UPDATE {MigrationConstants.PostCategoriesTableName}
            SET post_sorting_method_id = 1
            WHERE use_date_created_for_sorting = 0;
        """);
    }

    public static void DropUseDateCreatedForSortingColumn(this IDeleteExpressionRoot delete)
    {
        delete.Column("use_date_created_for_sorting")
            .FromTable(MigrationConstants.PostCategoriesTableName);
    }

    public static void RecreateUseDateCreatedForSortingColumn(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.PostCategoriesTableName)
            .AddColumn("use_date_created_for_sorting")
            .AsBoolean()
            .NotNullable()
            .WithDefaultValue(false);
    }

    public static void PopulateUseDateCreatedForSortingFromSortingMethod(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            UPDATE {MigrationConstants.PostCategoriesTableName}
            SET use_date_created_for_sorting = 1
            WHERE post_sorting_method_id = 2;
        """);

        execute.Sql($"""
            UPDATE {MigrationConstants.PostCategoriesTableName}
            SET use_date_created_for_sorting = 0
            WHERE post_sorting_method_id = 1;
        """);
    }

    public static void DeletePostSortingMethodForeignKey(this IDeleteExpressionRoot delete)
    {
        delete.ForeignKey(MigrationConstants.CategorySortingMethodForeignKeyName)
            .OnTable(MigrationConstants.PostCategoriesTableName);
    }

    public static void DropPostSortingMethodColumn(this IDeleteExpressionRoot delete)
    {
        delete.Column("post_sorting_method_id")
            .FromTable(MigrationConstants.PostCategoriesTableName);
    }
}
