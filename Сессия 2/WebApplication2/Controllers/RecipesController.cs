using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public RecipesController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            return await _context.Recipes.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Recipe>> GetRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound(new { message = $"Рецептура с id {id} не найдена" });
            return recipe;
        }

        [HttpGet("product/{productId}")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByProduct(int productId)
        {
            var recipes = await _context.Recipes.Where(r => r.ProductId == productId).ToListAsync();
            return recipes;
        }

        [HttpGet("product/{productId}/active")]
        public async Task<ActionResult<Recipe>> GetActiveRecipeByProduct(int productId)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.ProductId == productId && r.Status == "active");
            if (recipe == null)
                return NotFound(new { message = $"Активная рецептура для продукта {productId} не найдена" });
            return recipe;
        }

        [HttpPost]
        public async Task<ActionResult<Recipe>> CreateRecipe(Recipe recipe)
        {
            recipe.CreatedAt = DateTime.Now;
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRecipe), new { id = recipe.Id }, recipe);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound(new { message = $"Рецептура с id {id} не найдена" });

            // Деактивируем предыдущую активную рецептуру для этого продукта
            if (recipe.ProductId.HasValue)
            {
                var activeRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.ProductId == recipe.ProductId && r.Status == "active");
                if (activeRecipe != null)
                    activeRecipe.Status = "archived";
            }

            recipe.Status = "active";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Рецептура утверждена", recipe });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRecipe(int id, Recipe recipe)
        {
            if (id != recipe.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existingRecipe = await _context.Recipes.FindAsync(id);
            if (existingRecipe == null)
                return NotFound(new { message = $"Рецептура с id {id} не найдена" });

            existingRecipe.Name = recipe.Name;
            existingRecipe.Version = recipe.Version;
            existingRecipe.ProductId = recipe.ProductId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null)
                return NotFound(new { message = $"Рецептура с id {id} не найдена" });

            recipe.Status = "archived";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Рецептура заархивирована", recipe });
        }
    }
}