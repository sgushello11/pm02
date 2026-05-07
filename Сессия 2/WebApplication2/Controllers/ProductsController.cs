using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public ProductsController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = $"Продукт с id {id} не найден" });
            return product;
        }

        [HttpGet("code/{code}")]
        public async Task<ActionResult<Product>> GetProductByCode(string code)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Code == code);
            if (product == null)
                return NotFound(new { message = $"Продукт с кодом {code} не найден" });
            return product;
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByStatus(string status)
        {
            var products = await _context.Products.Where(p => p.Status == status).ToListAsync();
            return products;
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Code == product.Code);
            if (existingProduct != null)
                return BadRequest(new { message = $"Продукт с кодом {product.Code} уже существует" });

            product.CreatedAt = DateTime.Now;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (id != product.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existingProduct = await _context.Products.FindAsync(id);
            if (existingProduct == null)
                return NotFound(new { message = $"Продукт с id {id} не найден" });

            existingProduct.Name = product.Name;
            existingProduct.ProductType = product.ProductType;
            existingProduct.Form = product.Form;
            existingProduct.Status = product.Status;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = $"Продукт с id {id} не найден" });

            product.Status = "archived";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Продукт архивирован", product });
        }
    }
}