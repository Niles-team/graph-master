using System.Threading.Tasks;
using graph_master.data.interfaces;
using graph_master.models;

namespace graph_master.services
{
    public class UserService
    {
        private readonly IUserDao _userDao;

        public UserService(IUserDao userDao)
        {
            _userDao = userDao;
        }

        public async Task<User> CreateUser(User user) => await _userDao.CreateUser(user);

        public async Task<User> UpdateUser(User user) => await _userDao.UpdateUser(user);

        public async Task<UserAuthenticate> ValidateUser(string userName, string password) => await _userDao.ValidateUser(userName, password);
    }
}