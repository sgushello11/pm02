using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeComponentsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public RecipeComponentsController(AgroControlDbContext context)
        {
            _context = context;
        }

        // GET: api/recipecomponents/recipe/1
        [HttpGet("recipe/{recipeId}")]
        public async Task<ActionResult<IEnumerable<RecipeComponent>>> GetComponentsByRecipe(int recipeId)
        {
            var components = await _context.RecipeComponents
                .Where(rc => rc.RecipeId == recipeId)
                .OrderBy(rc => rc.OrderIndex)
                .ToListAsync();
            return components;
        }

        // POST: api/recipecomponents
        [HttpPost]
        public async Task<ActionResult<RecipeComponent>> AddComponent(RecipeComponent component)
        {
            // Проверка существования рецептуры
            var recipe = await _context.Recipes.FindAsync(component.RecipeId);
            if (recipe == null)
                return BadRequest(new { message = $"Рецептура с id {component.RecipeId} не найдена" });

            // Проверка существования материала
            var material = await _context.Materials.FindAsync(component.MaterialId);
            if (material == null)
                return BadRequest(new { message = $"Материал с id {component.MaterialId} не найден" });

            _context.RecipeComponents.Add(component);
            await _context.SaveChangesAsync();

            // Проверка суммы компонентов
            var total = await _context.RecipeComponents
                .Where(rc => rc.RecipeId == component.RecipeId)
                .SumAsync(rc => rc.Percentage);

            return Ok(new { message = "Компонент добавлен", component, totalPercent = total });
        }

        // PUT: api/recipecomponents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComponent(int id, RecipeComponent component)
        {
            if (id != component.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existing = await _context.RecipeComponents.FindAsync(id);
            if (existing == null)
                return NotFound(new { message = $"Компонент с id {id} не найден" });

            existing.Percentage = component.Percentage;
            existing.Tolerance = component.Tolerance;
            existing.OrderIndex = component.OrderIndex;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/recipecomponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComponent(int id)
        {
            var component = await _context.RecipeComponents.FindAsync(id);
            if (component == null)
                return NotFound(new { message = $"Компонент с id {id} не найден" });

            _context.RecipeComponents.Remove(component);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Компонент удален" });
        }

        // GET: api/recipecomponents/recipe/1/sum
        [HttpGet("recipe/{recipeId}/sum")]
        public async Task<ActionResult<object>> GetComponentsSum(int recipeId)
        {
            var total = await _context.RecipeComponents
                .Where(rc => rc.RecipeId == recipeId)
                .SumAsync(rc => rc.Percentage);

            var isValid = Math.Abs(total - 100) < 0.01m;

            return Ok(new { recipeId, totalPercent = total, isValid, message = isValid ? "Сумма корректна (100%)" : $"Сумма {total}%, требуется 100%" });
        }
    }
}