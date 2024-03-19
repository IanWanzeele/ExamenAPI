using ExamenNicIan.Core;
using ExamenNicIan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExamenNicIan.Services
{
    public class UserService : Controller
    {
        private readonly UserDbContext _dbContext;

        public UserService(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User>GetUser(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> Login(Login model)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
            return user;
        }
        public async Task Register(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
