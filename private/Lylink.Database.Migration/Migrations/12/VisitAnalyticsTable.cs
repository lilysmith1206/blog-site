using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;

namespace Lylink.Database.Migration._12;

public static class VisitAnalyticsTable
{
    public static void DropSlugGivenColumn(this IDeleteExpressionRoot delete)
    {
        delete.Column("slug_given")
            .FromTable(MigrationConstants.VisitAnalyticsTable);
    }

    public static void CreateSlugGivenColumn(this ICreateExpressionRoot create)
    {
        create.Column("slug_given")
            .OnTable(MigrationConstants.VisitAnalyticsTable);
    }

    public static void SetSlugGivenToSlugVisited(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            UPDATE {MigrationConstants.VisitAnalyticsTable}
            SET slug_visited = slug_given;
        """);
    }
}
