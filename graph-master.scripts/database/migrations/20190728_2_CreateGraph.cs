using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201907280002)]
    public class CreateGraph : Migration
    {
        public override void Up()
        {
            Create.Table("Graphs")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("DateUpdated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.Table("Nodes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("GraphId").AsInt32().NotNullable()
                .WithColumn("Data").AsString().Nullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("DateUpdated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);

            Create.Table("Links")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("GraphId").AsInt32().NotNullable()
                .WithColumn("Data").AsString().Nullable()
                .WithColumn("DateCreated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("DateUpdated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        public override void Down()
        {
            Delete.Table("Graphs");
            Delete.Table("Nodes");
            Delete.Table("Links");
        }
    }
}