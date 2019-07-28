using System;
using FluentMigrator.Runner;
using graph_master.scripts.utils;
using Microsoft.Extensions.DependencyInjection;

namespace graph_master.scripts.database
{
    public class Rollback : BaseTask
    {
        public static int Run()
        {
            Console.WriteLine("Rolling back the DB");
            var serviceProvider = MigrationUtils.CreateServices();
            using (var scope = serviceProvider.CreateScope())
            {
                RollbackDatabase(scope.ServiceProvider);
            }
            return 0;
        }

        private static void RollbackDatabase(IServiceProvider serviceProvider)
        {
            // Instantiate the runner
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            // Execute the migrations
            runner.Rollback(1);
        }
    }
}