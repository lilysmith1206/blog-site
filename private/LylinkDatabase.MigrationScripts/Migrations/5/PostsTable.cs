using FluentMigrator.Builders.Alter;

namespace LylinkDatabase.MigrationScripts.Migrations._5;

public static class PostsTable
{
    public static void AddNullableOptionsOnPosts(this IAlterExpressionRoot alter)
    {
        alter.Table(MigrationConstants.PostsTableName)
            .AlterColumn("title")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .AlterColumn("date_modified")
                .AsDateTime()
                .Nullable()
            .AlterColumn("name")
                .AsFixedLengthString(size: 80)
                .Nullable()
            .AlterColumn("keywords")
                .AsFixedLengthString(size: 160)
                .Nullable()
            .AlterColumn("description")
                .AsFixedLengthString(size: 160)
                .Nullable()
            .AlterColumn("body")
                .AsString(size: 60000)
                .Nullable()
            .AlterColumn("date_created")
                .AsDateTime()
                .Nullable()
            .AlterColumn("is_draft")
                .AsBoolean()
                .Nullable();
    }

    public static void RemoveNullableOptionsFromPosts(this IAlterExpressionRoot alter)
    {
        /*ALTER TABLE posts MODIFY title CHAR(80) NOT NULL;
ALTER TABLE posts MODIFY date_modified DATETIME NOT NULL;
ALTER TABLE posts MODIFY name CHAR(80) NOT NULL;
ALTER TABLE posts MODIFY keywords CHAR(160) NOT NULL;
ALTER TABLE posts MODIFY description CHAR(160) NOT NULL;
ALTER TABLE posts MODIFY body VARCHAR(60000) NOT NULL;
ALTER TABLE posts MODIFY date_created DATETIME NOT NULL;
UPDATE posts SET is_draft = 1 WHERE is_draft IS NULL;
ALTER TABLE posts MODIFY is_draft TINYINT(1) NOT NULL;
);*/
        alter.Table(MigrationConstants.PostsTableName)
            .AlterColumn("title")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .AlterColumn("date_modified")
                .AsDateTime()
                .NotNullable()
            .AlterColumn("name")
                .AsFixedLengthString(size: 80)
                .NotNullable()
            .AlterColumn("keywords")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .AlterColumn("description")
                .AsFixedLengthString(size: 160)
                .NotNullable()
            .AlterColumn("body")
                .AsString(size: 60000)
                .NotNullable()
            .AlterColumn("date_created")
                .AsDateTime()
                .NotNullable()
            .AlterColumn("is_draft")
                .AsBoolean()
                .NotNullable();

    }
}
