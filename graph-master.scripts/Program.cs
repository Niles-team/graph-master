using CommandLine;
using graph_master.common;
using graph_master.scripts.database;

namespace graph_master.scripts
{
    [Verb("migrate", HelpText = "Migrate the DB schema to the latest version")]
    class MigrateOptions
    {
    }

    class Program
    {
        static void Main(string[] args)
        {
            AppConfig.LoadEnv("../");

            CommandLine.Parser.Default.ParseArguments<MigrateOptions, RollbackOptions, DropOptions, CreateOptions>(args)
            .MapResult(
                (MigrateOptions opts) => RunMigrate(opts),
                errs => 1);
        }

        static int RunMigrate(MigrateOptions opts) =>
            Migrate.Run();
    }
}
