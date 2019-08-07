using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201908060001)]
    public class AddNotConfirmedUsers : Migration
    {
        public override void Up()
        {
            Create.Table("not_confirmed_users")
                .WithColumn("user_id").AsInt32().PrimaryKey()
                .WithColumn("confirm_code").AsGuid().NotNullable()
                .WithColumn("date_created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }

        public override void Down()
        {
            Delete.Table("not_confirmed_users");
        }
    }
}