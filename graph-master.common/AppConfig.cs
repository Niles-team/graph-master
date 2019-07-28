using System;
using System.IO;
using graph_master.common.Exceptions;

namespace graph_master.common
{
    ///<summary>App configurations class</summary>
    public static class AppConfig
    {
        ///<summary>Database connection string</summary>
        public static string DbConnectionString
        {
            get
            {
                return $"Host={DbHost};port={DbPort};Username={DbUsername};Password={DbPassword};Database={DbName};";
            }
        }

        ///<summary>Database name for connection</summary>
        public static string DbName
        {
            get { return GetRequiredVar("MAIN_DB_NAME"); }
        }

        ///<summary>Host for connection to database</summary>
        public static string DbHost
        {
            get { return GetRequiredVar("MAIN_DB_HOST"); }
        }

        ///<summary>Port for connection to database</summary>
        public static string DbPort
        {
            get { return DefaultedEnvVar("MAIN_DB_PORT", "5432"); }
        }

        ///<summary>User name for connection to database</summary>
        public static string DbUsername
        {
            get { return GetRequiredVar("MAIN_DB_USERNAME"); }
        }

        ///<summary>Password for connection to database</summary>
        public static string DbPassword
        {
            get { return DefaultedEnvVar("MAIN_DB_PASSWORD", ""); }
        }

        ///<summary>Need to show database logs</summary>
        public static bool ShowDbLogs
        {
            get
            {
                string value = DefaultedEnvVar("SHOW_DB_LOGS", "");
                return !string.IsNullOrEmpty(value);
            }
        }

        ///<summary>Name of environment of solution</summary>
        public static string EnvironmentName
        {
            get { return GetRequiredVar("ASPNETCORE_ENVIRONMENT"); }
        }

        ///<summary>Loading solution environment variables</summary>
        ///<param name="projectRoot">Path to the project root</param>
        public static void LoadEnv(string projectRoot)
        {
            var envFile = Path.Combine(projectRoot, ".env");
            if (File.Exists(envFile))
            {
                Console.WriteLine($"Loading environment config from {envFile}");
                DotNetEnv.Env.Load(envFile);
                return;
            }
            Console.WriteLine("No .env file found");
        }

        ///<summary>Get environment variable</summary>
        ///<param name="varName">Environment variable name</param>
        ///<returns>Founded environment variable value</returns>
        public static string GetEnvVar(string varName)
        {
            return Environment.GetEnvironmentVariable(varName);
        }

        ///<summary>Get environment variable with default value if variable doesn't exists</summary>
        ///<param name="varName">Environment variable name</param>
        ///<returns>Founded environment variable value</returns>
        public static string DefaultedEnvVar(string varName, string defaultVal)
        {
            var value = GetEnvVar(varName);
            return value == null ? defaultVal : value;
        }

        ///<summary>Get required for solution environment variable</summary>
        ///<param name="varName">Environment variable name</param>
        ///<exception cref="graph_master.common.Exceptions.EnvironmentVariableNotFound">Thrown when required for solution environment variable not founded</exception>
        ///<returns>Founded environment variable value</returns>
        public static string GetRequiredVar(string varName)
        {
            var value = GetEnvVar(varName);
            if (value == null)
            {
                string message = $"Variable '{varName}' is a required environment variable";
                throw new EnvironmentVariableNotFound(message);
            }
            return value;
        }
    }
}