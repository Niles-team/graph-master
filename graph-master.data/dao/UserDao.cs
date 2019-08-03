using System;
using System.Threading.Tasks;
using graph_master.data.interfaces;
using graph_master.models;

namespace graph_master.data.dao
{
    public class UserDao : BaseDao, IUserDao
    {
        public UserDao(string connectionString) : base(connectionString) { }

        public async Task<User> CreateUser(User user)
        {
            user.Id = await QueryFirstAsync<int>(@"
                insert into users (team_id, user_name, password_hash, first_name, last_name, email)
                values (@teamId, @userName, crypt(@password, gen_salt('bf')), @firstName, @lastName, @email)
                returning id
            ",
            new
            {
                teamId = user.TeamId,
                userName = user.UserName,
                password = user.Password,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email
            });
            return user;
        }

        public async Task<User> UpdateUser(User user)
        {
            await ExecuteAsync(@"
                update users
                set 
                    team_id = @teamId,
                    user_name = @userName,
                    password_hash = crypt(@password, gen_salt('bf')),
                    first_name = @firstName,
                    last_name = @lastName,
                    email = @email)
                where id = @id
            ",
            new
            {
                id = user.Id,
                teamId = user.TeamId,
                userName = user.UserName,
                password = user.Password,
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.Email
            });

            return user;
        }

        public async Task<UserAuthenticate> ValidateUser(string userName, string password)
        {
            return await QueryFirstAsync<UserAuthenticate>(@"
            select 
                team_id as teamId,
                user_name as userName, 
                first_name as firstName, 
                last_name as lastName, 
                email
            from users
            where user_name = @userName 
            and crypto(password, gen_salt('bf')) = password
            ", new { userName, password });
        }
    }
}