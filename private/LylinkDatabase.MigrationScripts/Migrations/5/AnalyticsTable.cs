using FluentMigrator.Builders.Alter;

namespace LylinkDatabase.MigrationScripts.Migrations._5;

public static class AnalyticsTable
{
    public static void AddNullableOptionsOnAnalytics(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.AnalyticsTableName)
            .AlterColumn("visitor_id")
                .AsFixedLengthString(size: 128)
                .Nullable()
            .AlterColumn("slug_visited")
                .AsFixedLengthString(size: 40)
                .Nullable()
            .AlterColumn("slug_given")
                .AsFixedLengthString(size: 40)
                .Nullable()
            .AlterColumn("visited_on")
                .AsDateTime()
                .Nullable();
    }

    public static void RemoveNullableOptionsFromAnalytics(this IAlterExpressionRoot alter)
    {
        /*CREATE TABLE IF NOT EXISTS visit_analytics (
ALTER TABLE visit_analytics MODIFY visitor_id CHAR(128) NOT NULL;
ALTER TABLE visit_analytics MODIFY slug_visited TEXT NOT NULL;
ALTER TABLE visit_analytics MODIFY slug_given CHAR(40) NOT NULL;
ALTER TABLE visit_analytics MODIFY visited_on DATETIME NOT NULL;
);*/
        alter.Table(MigrationConstants.AnalyticsTableName)
            .AlterColumn("visitor_id")
                .AsFixedLengthString(size: 128)
                .NotNullable()
            .AlterColumn("slug_visited")
                .AsFixedLengthString(size: 40)
                .NotNullable()
            .AlterColumn("slug_given")
                .AsFixedLengthString(size: 40)
                .NotNullable()
            .AlterColumn("visited_on")
                .AsDateTime()
                .NotNullable();
    }
}
