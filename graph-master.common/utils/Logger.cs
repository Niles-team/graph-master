using System;

namespace graph_master.common.utilities
{
    public static class Logger
    {
        public static void LogInfo(string message)
        {
            Console.WriteLine(message);
        }

        public static void LogException(Exception exception)
        {
            Console.WriteLine($"Exception was thrown. Exception code: {exception.HResult}. Exception message: {exception.Message}");
        }
    }
}