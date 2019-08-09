using System;
using System.Threading.Tasks;

using graph_master.models;

namespace graph_master.data.interfaces
{
    public interface IUserDao
    {
        Task<string> ValidateUserName(string userName);
        Task<string> ValidateEmail(string email);
        Task<User> GetUser(int id);
        Task<User> CreateUser(User user);
        Task<Guid> CreateConfirmCode(int userId);
        Task<User> ConfirmUser(Guid userCode);
        Task<User> UpdateUser(User user);
        Task<UserAuthenticated> SignIn(string userName, string password);
    }
}