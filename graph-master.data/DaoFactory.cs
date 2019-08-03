using graph_master.data.dao;
using graph_master.data.interfaces;

namespace graph_master.data
{
    public class DaoFactory : IDaoFactory
    {
        private readonly string connectionString;

        public DaoFactory(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public IUserDao UserDao => new UserDao(connectionString); 
    }
}