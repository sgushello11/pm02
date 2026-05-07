using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public UsersController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = $"Пользователь с id {id} не найден" });
            return user;
        }

        [HttpGet("username/{username}")]
        public async Task<ActionResult<Users>> GetUserByUsername(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return NotFound(new { message = $"Пользователь с логином {username} не найден" });
            return user;
        }

        [HttpGet("role/{role}")]
        public async Task<ActionResult<IEnumerable<Users>>> GetUsersByRole(string role)
        {
            var users = await _context.Users.Where(u => u.Role == role).ToListAsync();
            return users;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Users user)
        {
            if (id != user.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existingUser = await _context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound(new { message = $"Пользователь с id {id} не найден" });

            existingUser.FullName = user.FullName;
            existingUser.Role = user.Role;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.IsActive = user.IsActive;
            existingUser.Department = user.Department;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return NotFound(new { message = $"Пользователь с id {id} не найден" });

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Пользователь деактивирован", user });
        }
    }
}