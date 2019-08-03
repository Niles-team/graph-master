using System.Threading.Tasks;

using graph_master.models;

namespace graph_master.data.interfaces
{
    public interface IUserDao
    {
        Task<int> CreateUser(User user);
        Task<int> UpdateUser(User user);
        Task<UserAuthenticate> ValidateUser(string userName, string password);
    }
}