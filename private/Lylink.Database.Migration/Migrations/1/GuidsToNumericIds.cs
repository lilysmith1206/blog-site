using FluentMigrator;

namespace Lylink.Database.Migration._1;

[Migration(version: 1)]
public class GuidsToNumericIds : FluentMigrator.Migration
{
    public override void Down()
    {
        Create.CreateTemporaryPostHierarchyTableWithGuidIds();
        Create.CreateTemporaryPostsTableWithGuidIds();

        MigrateData();

        Delete.DeletePostParentForeignKey();

        Delete.Table(MigrationConstants.PostHierarchyTableName);
        Delete.Table(MigrationConstants.PostsTableName);

        Execute.RenameTempPostsToPosts();
        Execute.RenameTempPostHierarchyTableToPostHiearchyTable();

        Create.CreateForeignKeyForPostParent();
        Create.CreateForeignKeyForCategoryParent();
    }

    public override void Up()
    {
        Create.CreateTemporaryPostHierarchyTableWithIntegerIds();
        Create.CreateTemporaryPostsTableWithIntegerIds();

        MigrateData();

        Delete.DeletePostParentForeignKey();

        Delete.Table(MigrationConstants.PostHierarchyTableName);
        Delete.Table(MigrationConstants.PostsTableName);

        Execute.RenameTempPostsToPosts();
        Execute.RenameTempPostHierarchyTableToPostHiearchyTable();

        Create.CreateForeignKeyForPostParent();
        Create.CreateForeignKeyForCategoryParent();
    }

    private void MigrateData()
    {
        Execute.CopyExistingPostHierarchyDataToTempTable();
        Execute.MapExistingCategoryParentIdsToTempCategoryParentIds();

        Execute.CopyExistingPostsDataToTempTable();
        Execute.MapExistingPostParentsToTempHierarchyParents();
    }
}
