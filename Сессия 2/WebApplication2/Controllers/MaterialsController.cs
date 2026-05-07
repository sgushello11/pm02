using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public MaterialsController(AgroControlDbContext context)
        {
            _context = context;
        }

        // GET: api/materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterials()
        {
            return await _context.Materials.ToListAsync();
        }

        // GET: api/materials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
                return NotFound(new { message = $"Материал с id {id} не найден" });
            return material;
        }

        // GET: api/materials/code/PAV-001
        [HttpGet("code/{code}")]
        public async Task<ActionResult<Material>> GetMaterialByCode(string code)
        {
            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Code == code);
            if (material == null)
                return NotFound(new { message = $"Материал с кодом {code} не найден" });
            return material;
        }

        // GET: api/materials/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<Material>>> GetActiveMaterials()
        {
            return await _context.Materials.Where(m => m.IsActive).ToListAsync();
        }

        // POST: api/materials
        [HttpPost]
        public async Task<ActionResult<Material>> CreateMaterial(Material material)
        {
            var existing = await _context.Materials.FirstOrDefaultAsync(m => m.Code == material.Code);
            if (existing != null)
                return BadRequest(new { message = $"Материал с кодом {material.Code} уже существует" });

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, material);
        }

        // PUT: api/materials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, Material material)
        {
            if (id != material.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existing = await _context.Materials.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Материал с id {id} не найден" });

            existing.Name = material.Name;
            existing.Unit = material.Unit;
            existing.DefaultMinValue = material.DefaultMinValue;
            existing.DefaultMaxValue = material.DefaultMaxValue;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/materials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
                return NotFound(new { message = $"Материал с id {id} не найден" });

            material.IsActive = false;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Материал деактивирован", material });
        }
    }
}