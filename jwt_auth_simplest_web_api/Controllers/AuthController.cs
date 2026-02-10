using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using jwt_auth_simplest_web_api.Entities;
using jwt_auth_simplest_web_api.Models;
using jwt_auth_simplest_web_api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace jwt_auth_simplest_web_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto requestUserDto)
        {
            var user = await authService.RegisterAsync(requestUserDto);

            if (user is null)
                return BadRequest("User Already Exists");

            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto requestUserDto)
        {
            var token = await authService.LoginAsync(requestUserDto);
            if (token is null) return BadRequest("Invalid username or password");

            return Ok(token);
        }


        
    }
}
