using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;

namespace LylinkDatabase.MigrationScripts.Migrations._10;

public static class FailedVisitAnalyticsTable
{
    public static void CreateFailedVisitTable(this ICreateExpressionRoot create)
    {
        // structure of data recorded:
        // - session id
        // - attempted page to visit
        // - redirected to (expected to be 404)
        // - date it occurred on.

        create.Table(MigrationConstants.FailedVisitAnalyticsTable)
            .WithColumn("id")
                .AsInt32()
                .PrimaryKey()
                .Identity()
            .WithColumn("session_id")
                .AsFixedLengthString(size: 128)
                .NotNullable()
            .WithColumn("attempted_slug")
                .AsString()
                .NotNullable()
            .WithColumn("redirected_slug")
                .AsString()
                .NotNullable()
            .WithColumn("date_created")
                .AsDateTime()
                .NotNullable();
    }

    public static void DropFailedVisitTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(MigrationConstants.FailedVisitAnalyticsTable);
    }
}
