namespace graph_master.common.utilities
{
     public class EnvironmentUtils {
        static string PRODUCTION = "Production";
        static string STAGING = "Staging";

        static string DEVELOPMENT = "Development";
        static string TEST = "Test";

        ///<summary>Is application in production mode</summary>
        public static bool IsProduction() => IsEnvironmentEquals(PRODUCTION);
        ///<summary>Is application in staging mode</summary>
        public static bool IsStaging() => IsEnvironmentEquals(STAGING);
        ///<summary>Is application in development mode</summary>
        public static bool IsDevelopment() => IsEnvironmentEquals(DEVELOPMENT);
        ///<summary>Is application in test mode</summary>
        public static bool IsTest() => IsEnvironmentEquals(TEST);

        ///<summary>Is environment equals</summary>
        static bool IsEnvironmentEquals(string name) {
            var envName = AppConfig.EnvironmentName;
            return name == envName;
        }
    }
}