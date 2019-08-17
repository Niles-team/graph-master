using System;
using FluentMigrator.Runner;
using graph_master.scripts.utilities;
using Microsoft.Extensions.DependencyInjection;

namespace graph_master.scripts.database
{
    public class Migrate : BaseTask
    {
        public static int Run()
        {
            Console.WriteLine("Migrating the DB");
            var serviceProvider = MigrationUtils.CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
            return 0;
        }

        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.MigrateUp();
        }
    }    
}