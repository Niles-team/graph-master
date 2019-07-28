namespace graph_master.common.Exceptions
{
    public class EnvironmentVariableNotFound : System.Exception
    {
        public EnvironmentVariableNotFound() : base() { }
        public EnvironmentVariableNotFound(string message) : base(message) { }
    }
}