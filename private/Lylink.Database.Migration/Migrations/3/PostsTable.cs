using FluentMigrator.Builders.Alter;
using FluentMigrator.Builders.Delete;

namespace Lylink.Database.Migration._3;

public static class PostsTable
{
    public static void AddIsDraftColumn(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.PostsTableName)
            .AddColumn(MigrationConstants.IsDraftColumnName)
                .AsBoolean()
                .WithDefaultValue(true);
    }

    public static void RemoveIsDraftColumn(this IDeleteExpressionRoot delete)
    {
        delete.Column(MigrationConstants.IsDraftColumnName)
            .FromTable(MigrationConstants.PostsTableName);
    }
}
