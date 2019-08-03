using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201907280004)]
    public class AddGraphCreatorId : Migration
    {
        public override void Up()
        {
            Create.Column("UserId").OnTable("Graphs").AsInt32().Nullable();
            Create.Column("TeamId").OnTable("Graphs").AsInt32().Nullable();
        }
        public override void Down()
        {
            Delete.Column("UserId").FromTable("Graphs");
            Delete.Column("TeamId").FromTable("Graphs");
        }
    }
}