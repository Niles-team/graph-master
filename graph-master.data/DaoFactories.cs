using graph_master.data.interfaces;
using graph_master.models.enums;

namespace graph_master.data
{
    public class DaoFactories
    {
        public static IDaoFactory GetFactory(DataProvider provider, string connectionString)
        {
            switch (provider)
            {
                case DataProvider.Npgsql:
                    return new DaoFactory(connectionString);
                default:
                    return new DaoFactory(connectionString);
            }
        }
    }
}