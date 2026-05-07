using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechMapsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public TechMapsController(AgroControlDbContext context)
        {
            _context = context;
        }

        // GET: api/techmaps
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TechMap>>> GetTechMaps()
        {
            return await _context.TechMaps.ToListAsync();
        }

        // GET: api/techmaps/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TechMap>> GetTechMap(int id)
        {
            var techMap = await _context.TechMaps.FindAsync(id);
            if (techMap == null)
                return NotFound(new { message = $"Тех. карта с id {id} не найдена" });
            return techMap;
        }

        // GET: api/techmaps/product/1
        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<TechMap>>> GetTechMapsByProduct(int productId)
        {
            var techMaps = await _context.TechMaps.Where(t => t.ProductId == productId).ToListAsync();
            return techMaps;
        }

        // GET: api/techmaps/product/1/active
        [HttpGet("product/{productId}/active")]
        public async Task<ActionResult<TechMap>> GetActiveTechMapByProduct(int productId)
        {
            var techMap = await _context.TechMaps.FirstOrDefaultAsync(t => t.ProductId == productId && t.Status == "active");
            if (techMap == null)
                return NotFound(new { message = $"Активная тех. карта для продукта {productId} не найдена" });
            return techMap;
        }

        // POST: api/techmaps
        [HttpPost]
        public async Task<ActionResult<TechMap>> CreateTechMap(TechMap techMap)
        {
            techMap.CreatedAt = DateTime.Now;
            _context.TechMaps.Add(techMap);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTechMap), new { id = techMap.Id }, techMap);
        }

        // PUT: api/techmaps/5/approve - утвердить карту
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveTechMap(int id, [FromBody] int approvedBy)
        {
            var techMap = await _context.TechMaps.FindAsync(id);
            if (techMap == null)
                return NotFound(new { message = $"Тех. карта с id {id} не найдена" });

            techMap.Status = "active";
            techMap.ApprovedAt = DateTime.Now;
            techMap.ApprovedBy = approvedBy;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Тех. карта утверждена", techMap });
        }

        // PUT: api/techmaps/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechMap(int id, TechMap techMap)
        {
            if (id != techMap.Id)
                return BadRequest(new { message = "ID не совпадают" });

            _context.Entry(techMap).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/techmaps/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechMap(int id)
        {
            var techMap = await _context.TechMaps.FindAsync(id);
            if (techMap == null)
                return NotFound(new { message = $"Тех. карта с id {id} не найдена" });

            _context.TechMaps.Remove(techMap);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Тех. карта удалена", techMap });
        }
    }
}