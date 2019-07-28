using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201907280001)]
    public class CreateGraph : Migration
    {
        public override void Up()
        {
            Create.Table("graphs")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("name").AsString().NotNullable()
                .WithColumn("date_created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("date_updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.Table("nodes")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("graph_id").AsInt32().NotNullable()
                .WithColumn("data").AsString().Nullable()
                .WithColumn("date_created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("date_updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.Table("links")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("graph_id").AsInt32().NotNullable()
                .WithColumn("data").AsString().Nullable()
                .WithColumn("date_created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("date_updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        public override void Down()
        {
            Delete.Table("graphs");
            Delete.Table("nodes");
            Delete.Table("links");
        }
    }
}