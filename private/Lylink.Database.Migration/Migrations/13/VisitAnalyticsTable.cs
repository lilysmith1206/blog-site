using FluentMigrator.Builders.Rename;

namespace Lylink.Database.Migration._13;

public static class VisitAnalyticsTable
{
    public static void RenameToMatchFailedVisitAnalytics(this IRenameExpressionRoot rename)
    {
        rename
            .Column("slug_visited")
            .OnTable(MigrationConstants.VisitAnalyticsTable)
            .To("visited_slug");

        rename
            .Column("visitor_id")
            .OnTable(MigrationConstants.VisitAnalyticsTable)
            .To("session_id");

        rename
            .Column("visited_on")
            .OnTable(MigrationConstants.VisitAnalyticsTable)
            .To("date_created");
    }

    public static void RenameToDifferFromFailedVisitAnalytics(this IRenameExpressionRoot rename)
    {
        rename
            .Column("visited_slug")
            .OnTable(MigrationConstants.VisitAnalyticsTable)
            .To("slug_visited");

        rename
            .Column("session_id")
            .OnTable(MigrationConstants.VisitAnalyticsTable)
            .To("visitor_id");

        rename
            .Column("date_created")
            .OnTable(MigrationConstants.VisitAnalyticsTable)
            .To("visited_on");
    }
}
