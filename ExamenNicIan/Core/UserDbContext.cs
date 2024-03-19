using Microsoft.EntityFrameworkCore;

namespace ExamenNicIan.Core
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
    }
}
