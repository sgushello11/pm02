using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public AuthController(AgroControlDbContext context)
        {
            _context = context;
        }

        // POST: api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<Users>> Register([FromBody] RegisterRequest request)
        {
            // Проверка существования пользователя
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
                return BadRequest(new { message = "Пользователь с таким логином уже существует" });

            // Хэширование пароля (простейшее, для production используйте BCrypt)
            var passwordHash = HashPassword(request.Password);

            var user = new Users
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                FullName = request.FullName,
                Role = request.Role ?? "operator",
                Email = request.Email,
                Phone = request.Phone,
                IsActive = true,
                Department = request.Department,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Регистрация успешна", userId = user.Id });
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);

            if (user == null || user.PasswordHash != HashPassword(request.Password))
                return Unauthorized(new { message = "Неверный логин или пароль" });

            if (!user.IsActive)
                return Unauthorized(new { message = "Пользователь заблокирован" });

            // Обновляем время последнего входа
            user.LastLogin = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Вход выполнен успешно",
                userId = user.Id,
                username = user.Username,
                fullName = user.FullName,
                role = user.Role,
                department = user.Department
            });
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }

    public class RegisterRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}