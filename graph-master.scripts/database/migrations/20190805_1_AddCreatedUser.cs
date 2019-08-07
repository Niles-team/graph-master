using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201908050001)]
    public class AddConfirmedUser : Migration
    {
        public override void Up()
        {
            Create.Column("confirmed").OnTable("users").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down() 
        {
            Delete.Column("confirmed").FromTable("users");
        }
    }
}