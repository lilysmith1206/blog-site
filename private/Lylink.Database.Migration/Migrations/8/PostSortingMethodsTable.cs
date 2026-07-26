using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Insert;

namespace Lylink.Database.Migration._8;

public static class PostSortingMethodsTable
{
    public static void CreatePostSortingMethodsTable(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.PostSortingMethodsTableName)
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("sorting_name").AsFixedLengthString(80).NotNullable();
    }

    public static void SeedPostSortingMethods(this IInsertExpressionRoot insert)
    {
        insert.IntoTable(MigrationConstants.PostSortingMethodsTableName)
            .Rows([
                new { id = 1, sorting_name = "ByDateCreatedAscending" },
                new { id = 2, sorting_name = "ByDateCreatedDescending" },
                new { id = 3, sorting_name = "ByDateModifiedAscending" },
                new { id = 4, sorting_name = "ByDateModifiedDescending" },
            ]);
    }

    public static void DropPostSortingMethodsTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(MigrationConstants.PostSortingMethodsTableName);
    }
}
