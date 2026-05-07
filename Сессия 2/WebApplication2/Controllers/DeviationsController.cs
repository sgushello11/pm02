using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviationsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public DeviationsController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deviation>>> GetDeviations()
        {
            return await _context.Deviations.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Deviation>> GetDeviation(int id)
        {
            var deviation = await _context.Deviations.FindAsync(id);
            if (deviation == null)
                return NotFound(new { message = $"Отклонение с id {id} не найдено" });
            return deviation;
        }

        [HttpGet("batch/{batchId}")]
        public async Task<ActionResult<IEnumerable<Deviation>>> GetByBatch(int batchId)
        {
            var deviations = await _context.Deviations.Where(d => d.BatchId == batchId).ToListAsync();
            return deviations;
        }

        [HttpGet("severity/{severity}")]
        public async Task<ActionResult<IEnumerable<Deviation>>> GetBySeverity(string severity)
        {
            var deviations = await _context.Deviations.Where(d => d.Severity == severity).ToListAsync();
            return deviations;
        }

        [HttpGet("unresolved")]
        public async Task<ActionResult<IEnumerable<Deviation>>> GetUnresolved()
        {
            var deviations = await _context.Deviations.Where(d => d.ResolvedAt == null).ToListAsync();
            return deviations;
        }

        [HttpPost]
        public async Task<ActionResult<Deviation>> CreateDeviation(Deviation deviation)
        {
            deviation.ReportedAt = DateTime.Now;
            _context.Deviations.Add(deviation);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetDeviation), new { id = deviation.Id }, deviation);
        }

        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> ResolveDeviation(int id, [FromBody] string comment)
        {
            var deviation = await _context.Deviations.FindAsync(id);
            if (deviation == null)
                return NotFound(new { message = $"Отклонение с id {id} не найдено" });

            deviation.ResolvedAt = DateTime.Now;
            deviation.ResolutionComment = comment;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Отклонение закрыто", deviation });
        }
    }
}