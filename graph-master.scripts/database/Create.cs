using System;
using System.Diagnostics;
using graph_master.common;

namespace graph_master.scripts.database
{    
    public class Create : BaseTask
    {
        public static int Run(bool allowProd = false)
        {
            Console.WriteLine("Creating the DB");
            GuardProd(allowProd: allowProd);
            var process = Process.Start("psql", $"-c \"create database {AppConfig.DbName}\"");
            process.WaitForExit();
            return 0;
        }
    }
}