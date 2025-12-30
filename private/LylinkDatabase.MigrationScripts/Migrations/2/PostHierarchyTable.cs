using FluentMigrator.Builders.Execute;

namespace LylinkDatabase.MigrationScripts.Migrations._2;

public static class PostHierarchyTable
{
    public static void RenameToPostCategory(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            RENAME TABLE {MigrationConstants.PostHierarchyTableName} TO {MigrationConstants.PostCategoryTableName};
        """);
    }

    public static void RenameToPostHierarchy(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            RENAME TABLE {MigrationConstants.PostCategoryTableName} TO {MigrationConstants.PostHierarchyTableName};
        """);
    }
}
