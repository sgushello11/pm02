using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public EquipmentController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetEquipment()
        {
            return await _context.Equipment.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> GetEquipment(int id)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
                return NotFound(new { message = $"Оборудование с id {id} не найдено" });
            return equipment;
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Equipment>> GetEquipmentByCode(string code)
        {
            var equipment = await _context.Equipment.FirstOrDefaultAsync(e => e.Code == code);
            if (equipment == null)
                return NotFound(new { message = $"Оборудование с кодом {code} не найдено" });
            return equipment;
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Equipment>>> GetByStatus(string status)
        {
            var equipment = await _context.Equipment.Where(e => e.Status == status).ToListAsync();
            return equipment;
        }

        [HttpPost]
        public async Task<ActionResult<Equipment>> CreateEquipment(Equipment equipment)
        {
            _context.Equipment.Add(equipment);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEquipment), new { id = equipment.Id }, equipment);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var equipment = await _context.Equipment.FindAsync(id);
            if (equipment == null)
                return NotFound(new { message = $"Оборудование с id {id} не найдено" });

            equipment.Status = status;
            await _context.SaveChangesAsync();
            return Ok(new { message = $"Статус обновлен на {status}", equipment });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEquipment(int id, Equipment equipment)
        {
            if (id != equipment.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existing = await _context.Equipment.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Оборудование с id {id} не найдено" });

            existing.Name = equipment.Name;
            existing.Location = equipment.Location;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}