using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201907280004)]
    public class AddGraphCreatorId : Migration
    {
        public override void Up()
        {
            Create.Column("user_id").OnTable("graphs").AsInt32().Nullable();
            Create.Column("team_id").OnTable("graphs").AsInt32().Nullable();
        }
        public override void Down()
        {
            Delete.Column("user_id").FromTable("graphs");
            Delete.Column("team_id").FromTable("graphs");
        }
    }
}