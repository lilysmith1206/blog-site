using FluentMigrator.Builders.Execute;

namespace LylinkDatabase.MigrationScripts.Migrations._10;

public static class VisitAnalyticsTable
{
    /// <summary>
    /// 404-rerouted visits are the only visit failure recorded at this point in time, so it's the ones copied over.
    /// </summary>
    public static void Copy404GivenVisitsToFailedTable(this IExecuteExpressionRoot execute)
    {
        execute.Sql($"""
            INSERT INTO {MigrationConstants.FailedVisitAnalyticsTable}
                (session_id, attempted_slug, redirected_slug, date_created)
            SELECT
                visitor_id,
                slug_visited,
                slug_given,
                visited_on
            FROM {MigrationConstants.VisitAnalyticsTable}
            WHERE slug_given = '404';
        """);
    }
}
