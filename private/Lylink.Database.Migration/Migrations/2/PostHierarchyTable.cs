using FluentMigrator.Builders.Execute;

namespace Lylink.Database.Migration._2;

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
