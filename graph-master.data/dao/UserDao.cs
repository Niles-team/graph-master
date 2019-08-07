using System;
using System.Threading.Tasks;
using graph_master.common.utils;
using graph_master.data.interfaces;
using graph_master.models;

namespace graph_master.data.dao
{
    public class UserDao : BaseDao, IUserDao
    {
        public UserDao(string connectionString) : base(connectionString) { }

        public async Task<User> CreateUser(User user)
        {
            try
            {
                Logger.LogInfo("Start user creating");

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

                Logger.LogInfo("User successful created");
                return user;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                throw exception;
            }
        }

        public async Task<Guid> CreateConfirmCode(int userId)
        {
            try
            {
                Logger.LogInfo("Start creating user confirm code.");
                Guid code = Guid.NewGuid();

                await ExecuteAsync(@"
                    insert into not_confirmed_users (user_id, confirm_code)
                    values (@userId, @code)
                ", new { userId, code });

                Logger.LogInfo("User confirm code successful created.");
                return code;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                throw exception;
            }
        }

        public async Task<bool> ConfirmUser(Guid confirmCode)
        {
            try
            {
                return await Task.Run(() =>
                {
                    Logger.LogInfo("Start user confirmation");

                    int? userId = QueryFirstOrDefault<int>(@"
                        select user_id
                        from not_confirmed_users
                        where confirm_code = @confirmCode
                    ");

                    if (userId.HasValue)
                    {
                        Logger.LogInfo("User successful found");

                        Execute(@"
                            delete from not_confirmed_users
                            where user_id = @userId
                        ", new { userId });

                        Logger.LogInfo("User successful confirmed");
                    }
                    else
                    {
                        Logger.LogInfo("Not confirmed user not found or already confirmed");
                    }

                    return userId.HasValue;
                });
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                throw exception;
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {
                Logger.LogInfo("Start updating user.");

                await ExecuteAsync(@"
                    update users
                    set 
                        team_id = @teamId,
                        user_name = @userName,
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

                Logger.LogInfo("User was successful updated.");
                return user;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                throw exception;
            }
        }

        public async Task<UserAuthenticated> SignIn(string userName, string password)
        {
            try
            {
                Logger.LogInfo("Start user sign in.");

                var result = await QueryFirstOrDefaultAsync<UserAuthenticated>(@"
                select 
                    id as Id,
                    team_id as TeamId,
                    user_name as UserName, 
                    first_name as FirstName, 
                    last_name as LastName, 
                    email as Email
                from users
                where user_name = @userName 
                and password_hash = crypt(@password, gen_salt('bf'))
                ", new { userName, password });

                Logger.LogInfo("User successful sign in.");

                return result;
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
                throw exception;
            }
        }
    }
}