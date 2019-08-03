namespace graph_master.data.interfaces
{
    public interface IDaoFactory
    {
        IUserDao UserDao { get; }
    }
}