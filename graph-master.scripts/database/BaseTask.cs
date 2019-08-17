using graph_master.common.utilities;

namespace graph_master.scripts.database
{
    public class OperationNotAllowedInProdException : System.Exception
    {
        public OperationNotAllowedInProdException() : base() { }
        public OperationNotAllowedInProdException(string message) : base(message) { }
    }

    public class BaseTask
    {
        protected static void GuardProd(bool allowProd = false)
        {
            if (!allowProd && EnvironmentUtils.IsProduction())
            {
                string message = "You cannot run this in production. Pass the --allow-prod flag to allow this";
                throw new OperationNotAllowedInProdException(message);
            }
        }
    }
}