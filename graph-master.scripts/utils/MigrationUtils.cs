using System;
using FluentMigrator.Runner;
using graph_master.common;
using graph_master.scripts.database.migrations;
using Microsoft.Extensions.DependencyInjection;

namespace graph_master.scripts.utils
{
    public class MigrationUtils
    {
       public static IServiceProvider CreateServices()
        {
            return new ServiceCollection()
                // Add common FluentMigrator services
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(AppConfig.DbConnectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(CreateUser).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                // Build the service provider
                .BuildServiceProvider(false);
        } 
    }
}