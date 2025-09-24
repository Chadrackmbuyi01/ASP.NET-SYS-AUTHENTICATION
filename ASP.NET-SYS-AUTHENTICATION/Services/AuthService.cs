using ASP.NET_SYS_AUTHENTICATION.Models;
using ASP.NET_SYS_AUTHENTICATION.Data;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_SYS_AUTHENTICATION.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(RegisterModel model);
        Task<User?> LoginAsync(LoginModel model);
        Task<User?> GetUserByIdAsync(int userId);

    }

    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;

        public AuthService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> RegisterAsync(RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return null; // User with the same email already exists
            }

            var user = new User
            {
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Firstname = model.Firstname,
                Lastname = model.Lastname,
                CreatedAt = DateTime.UtcNow
                
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginAsync(LoginModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return null; // Invalid email or password
            }

            user.LastLogin = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }
    }
}