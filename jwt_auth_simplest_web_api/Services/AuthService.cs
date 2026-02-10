using jwt_auth_simplest_web_api.Data;
using jwt_auth_simplest_web_api.Entities;
using jwt_auth_simplest_web_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace jwt_auth_simplest_web_api.Services
{
    public class AuthService(UserDbContext context, IConfiguration configuration) : IAuthService
    {
        public async Task<string?> LoginAsync(UserDto requestUserDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserName == requestUserDto.UserName);
            
            if (user is null)
                return null;

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, requestUserDto.Password) ==
                PasswordVerificationResult.Failed)
                return null;

            return CreateToken(user);
        }

        public async Task<User?> RegisterAsync(UserDto requestUserDto) {

            if (await context.Users.AnyAsync(u => u.UserName == requestUserDto.UserName))
                return null;

            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, requestUserDto.Password);
            user.UserName = requestUserDto.UserName;
            user.PasswordHash = hashedPassword;

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return user;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.UserName), new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Key")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Jwt:Issuer"),
                audience: configuration.GetValue<string>("Jwt:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}
