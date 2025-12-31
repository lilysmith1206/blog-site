using FluentMigrator.Builders.Delete;
using FluentMigrator.Builders.Execute;

namespace LylinkDatabase.MigrationScripts.Migrations._11;

public static class VisitAnalyticsTable
{
    public static void DropFailedVisitsFromVisitAnalytics(this IDeleteExpressionRoot delete)
    {
        delete.FromTable(MigrationConstants.VisitAnalyticsTable)
            .Row(new { slug_given = "404" }); 
    }

    public static void CopyFailedVisitsFromFailedVisitsTable(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            INSERT INTO {MigrationConstants.VisitAnalyticsTable}
                (visitor_id, slug_visited, slug_given, visited_on)
            SELECT
                session_id,
                attempted_slug,
                redirected_slug,
                date_created
            FROM {MigrationConstants.FailedVisitAnalyticsTable}
        """);
    }
}
