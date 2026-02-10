using jwt_auth_simplest_web_api.Entities;
using jwt_auth_simplest_web_api.Models;

namespace jwt_auth_simplest_web_api.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserDto requestUserDto);
        Task<string?> LoginAsync(UserDto requestUserDto);
    }
}
