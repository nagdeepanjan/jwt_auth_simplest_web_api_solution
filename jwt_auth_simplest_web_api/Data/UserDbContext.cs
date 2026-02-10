using jwt_auth_simplest_web_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace jwt_auth_simplest_web_api.Data
{
    public class UserDbContext(DbContextOptions<UserDbContext> options): DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
