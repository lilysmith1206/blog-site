using FluentMigrator.Builders.Alter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LylinkDatabase.MigrationScripts.Migrations._5;

public static class AnnotationsTable
{
    public static void AddNullableOptionsOnAnnotations(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.AnnotationsTableName)
            .AlterColumn("slug")
                .AsString(size: 40)
                .Nullable()
            .AlterColumn("editor_name")
                .AsString(size: 80)
                .Nullable()
            .AlterColumn("annotation_content")
                .AsString(size: 10000)
                .Nullable();
    }

    public static void RemoveNullableOptionsFromAnnotations(this IAlterExpressionRoot alter)
    {
        /*ALTER TABLE annotations MODIFY slug VARCHAR(40) NOT NULL;
ALTER TABLE annotations MODIFY editor_name VARCHAR(80) NOT NULL;
ALTER TABLE annotations MODIFY annotation_content VARCHAR(10000) NOT NULL;*/
        alter.Table(MigrationConstants.AnnotationsTableName)
            .AlterColumn("slug")
                .AsString(size: 40)
                .NotNullable()
            .AlterColumn("editor_name")
                .AsString(size: 80)
                .NotNullable()
            .AlterColumn("annotation_content")
                .AsString(size: 10000)
                .NotNullable();
    }
}
