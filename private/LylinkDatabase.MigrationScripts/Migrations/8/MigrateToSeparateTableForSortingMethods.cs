using FluentMigrator;

namespace LylinkDatabase.MigrationScripts.Migrations._8;

[Migration(version: 8)]
public class MigrateToSeparateTableForSortingMethods : Migration
{
    public override void Down()
    {
        Alter.RecreateUseDateCreatedForSortingColumn();
        Execute.PopulateUseDateCreatedForSortingFromSortingMethod();
        Delete.DeletePostSortingMethodForeignKey();
        Delete.DropPostSortingMethodColumn();
        Delete.DropPostSortingMethodsTable();
    }

    public override void Up()
    {
        Create.CreatePostSortingMethodsTable();
        Insert.SeedPostSortingMethods();
        Alter.AddPostSortingMethodColumn();
        Execute.PopulatePostSortingMethodIds();
        Delete.DropUseDateCreatedForSortingColumn();
    }
}
