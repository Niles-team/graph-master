using System.Threading.Tasks;

using graph_master.models;

namespace graph_master.data.interfaces
{
    public interface IUserDao
    {
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user);
        Task<UserAuthenticate> ValidateUser(string userName, string password);
    }
}