using FluentMigrator.Builders.Create;
using FluentMigrator.Builders.Delete;

namespace LylinkDatabase.MigrationScripts.Migrations._9;

public static class AnnotationsTable
{
    public static void DropAnnotationsTable(this IDeleteExpressionRoot delete)
    {
        delete.Table(MigrationConstants.AnnotationsTableName);
    }

    public static void CreateAnnotationsTable(this ICreateExpressionRoot create)
    {
        create.Table(MigrationConstants.AnnotationsTableName)
            .WithColumn("id")
                .AsString(size: 40)
                .NotNullable()
                .PrimaryKey()
            .WithColumn("slug")
                .AsString(size: 40)
                .Nullable()
            .WithColumn("editor_name")
                .AsString(size: 40)
                .Nullable()
            .WithColumn("annotation_content")
                .AsString(size: 10000)
                .Nullable();
    }
}
