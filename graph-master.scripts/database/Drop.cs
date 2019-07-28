using System;
using System.Diagnostics;
using graph_master.common;

namespace graph_master.scripts.database
{    
    public class Drop : BaseTask
    {
        public static int Run(bool allowProd = false)
        {
            Console.WriteLine("Dropping the DB");
            GuardProd(allowProd: allowProd);
            var process = Process.Start("psql", $"-c \"drop database {AppConfig.DbName}\"");
            process.WaitForExit();
            return 0;
        }
    }
}