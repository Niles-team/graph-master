using System;
using System.Threading.Tasks;
using graph_master.models;
using graph_master.services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace graph_master.api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet("validate-username")]
        public async Task<IActionResult> ValidateUserName([FromQuery]string userName)
        {
            var result = await _userService.ValidateUserName(userName);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("validate-email")]
        public async Task<IActionResult> ValidateEmail([FromQuery] string email)
        {
            var result = await _userService.ValidateEmail(email);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody]User model)
        {
            var result = await _userService.CreateUser(model);

            if(result == null) {
                return BadRequest(new { message = "There was some errors with user sign up" });
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody]UserSignIn model)
        {
            var result = await _userService.SignIn(model.UserName, model.Password);

            if (result == null)
                return Ok(new { message = "User name or password is incorrect" });

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("confirm-user")]
        public async Task<IActionResult> ConfirmUser([FromBody]UserConfirm model) 
        {
            var result = await _userService.ConfirmUser(model.Code);

            return Ok(result);
        }
    }

}