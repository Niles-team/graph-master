using System;
using FluentMigrator;

namespace graph_master.scripts.database.migrations
{
    [Migration(201907280001)]
    public class CreateUser : Migration
    {
        public override void Up()
        {
            Create.Table("users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("team_id").AsInt32().Nullable()
                .WithColumn("user_name").AsString().NotNullable()
                .WithColumn("password_hash").AsString().NotNullable()
                .WithColumn("first_name").AsString().NotNullable()
                .WithColumn("last_name").AsString().NotNullable()
                .WithColumn("email").AsString().NotNullable()
                .WithColumn("date_created").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime)
                .WithColumn("date_updated").AsDateTime().NotNullable().WithDefault(SystemMethods.CurrentUTCDateTime);
        }
        
        public override void Down()
        {
            Delete.Table("users");
        }
    }
}