using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;

namespace Lylink.Database.Migration._4;

public static class AnalyticsTable
{
    public static void RemoveAnalyticsTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(MigrationConstants.AnalyticsTableName);
    }

    public static void AddAnalyticsTable(this ICreateExpressionRoot create)
    {
        /*CREATE TABLE IF NOT EXISTS visit_analytics (
    id INT NOT NULL PRIMARY KEY AUTO_INCREMENT,
    visitor_id CHAR(128),
    slug_visited CHAR(40),
    slug_given CHAR(40),
    visited_on DATETIME
);*/
        create.Table(MigrationConstants.AnalyticsTableName)
            .WithColumn("id")
                .AsInt32()
                .NotNullable()
                .PrimaryKey()
                .Identity()
            .WithColumn("visitor_id")
                .AsFixedLengthString(size: 128)
                .Nullable()
            .WithColumn("slug_visited")
                .AsFixedLengthString(size: 40)
                .Nullable()
            .WithColumn("slug_given")
                .AsFixedLengthString(size: 40)
                .Nullable()
            .WithColumn("visited_on")
                .AsDateTime()
                .Nullable();
    }
}
