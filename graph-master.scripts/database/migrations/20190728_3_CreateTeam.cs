using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201907280003)]
    public class CreateTeam : Migration
    {
        public override void Up()
        {
            Create.Table("teams")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("description").AsString().Nullable()
                .WithColumn("url").AsString().Nullable()
                .WithColumn("date_created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("date_updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        public override void Down()
        {
            Delete.Table("teams");
        }
    }
}