using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using graph_master.models.helpers;
using graph_master.data.interfaces;
using graph_master.models;
using System.Text;
using System.Security.Claims;
using System;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace graph_master.services
{
    public class UserService
    {
        private readonly IUserDao _userDao;
        private readonly EmailService _emailService;
        private readonly AppSettings _settings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            IUserDao userDao,
            EmailService emailService,
            AppSettings settings,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userDao = userDao;
            _emailService = emailService;
            _settings = settings;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> CreateUser(User user)
        {
            var _user = user;//await _userDao.CreateUser(user);
            if (_user == null)
            {
                return null;
            }

            Guid confirmCode = await _userDao.CreateConfirmCode(user.Id);

            string url = string.Empty;
            if (_httpContextAccessor.HttpContext.Request.Headers.ContainsKey("Origin"))
            {
                var origin = _httpContextAccessor.HttpContext.Request.Headers["Origin"];
                url = $"{origin}/confirm-email/{confirmCode}";
            }
            else
            {
                Exception exception = new Exception("Cannot find origin resource");
                throw exception;
            }

            string subject = "Email Confirmation";
            string message = _emailService.CreateVerifyMailMessageContent(subject, "We just want to confirm that was you", url);
            await _emailService.SendEmailAsync(user.Email, subject, message);

            _user.Password = null;

            return _user;
        }

        public async Task<bool> ConfirmUser(Guid confirmCode)
        {
            var result = await _userDao.ConfirmUser(confirmCode);

            return result;
        }

        public async Task<User> UpdateUser(User user) => await _userDao.UpdateUser(user);

        public async Task<UserAuthenticated> SignIn(string userName, string password)
        {
            var user = await _userDao.SignIn(userName, password);
            if (user == null)
            {
                return null;
            }

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            // remove password before returning
            user.Password = null;

            return user;
        }
    }
}