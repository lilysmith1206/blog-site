using FluentMigrator;

namespace Lylink.Database.Migration._5;

[Migration(version: 5)]
public class RemoveNullableAsValidForTables : FluentMigrator.Migration
{
    public override void Down()
    {
        Alter.AddNullableOptionsOnAnalytics();
        Alter.AddNullableOptionsOnAnnotations();
        Alter.AddNullableOptionsOnPostCategories();
        Alter.AddNullableOptionsOnPosts();
    }

    public override void Up()
    {
        Alter.RemoveNullableOptionsFromAnalytics();
        Alter.RemoveNullableOptionsFromAnnotations();
        Alter.RemoveNullableOptionsFromPostCategories();
        Alter.RemoveNullableOptionsFromPosts();
    }
}
