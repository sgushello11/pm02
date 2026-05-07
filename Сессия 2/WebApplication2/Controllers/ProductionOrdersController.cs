using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionOrdersController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public ProductionOrdersController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionOrder>>> GetOrders()
        {
            return await _context.ProductionOrders.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionOrder>> GetOrder(int id)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null)
                return NotFound(new { message = $"Заказ с id {id} не найден" });
            return order;
        }

        [HttpGet("number/{orderNumber}")]
        public async Task<ActionResult<ProductionOrder>> GetOrderByNumber(string orderNumber)
        {
            var order = await _context.ProductionOrders.FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
            if (order == null)
                return NotFound(new { message = $"Заказ {orderNumber} не найден" });
            return order;
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<ProductionOrder>>> GetOrdersByStatus(string status)
        {
            var orders = await _context.ProductionOrders.Where(o => o.Status == status).ToListAsync();
            return orders;
        }

        [HttpPost]
        public async Task<ActionResult<ProductionOrder>> CreateOrder(ProductionOrder order)
        {
            order.CreatedAt = DateTime.Now;
            _context.ProductionOrders.Add(order);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartOrder(int id)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null)
                return NotFound(new { message = $"Заказ с id {id} не найден" });

            order.Status = "in_progress";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Заказ запущен", order });
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteOrder(int id)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null)
                return NotFound(new { message = $"Заказ с id {id} не найден" });

            order.Status = "completed";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Заказ завершен", order });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, ProductionOrder order)
        {
            if (id != order.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existingOrder = await _context.ProductionOrders.FindAsync(id);
            if (existingOrder == null)
                return NotFound(new { message = $"Заказ с id {id} не найден" });

            existingOrder.PlannedQuantityKg = order.PlannedQuantityKg;
            existingOrder.PlannedStartDate = order.PlannedStartDate;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.ProductionOrders.FindAsync(id);
            if (order == null)
                return NotFound(new { message = $"Заказ с id {id} не найден" });

            order.Status = "archived";
            await _context.SaveChangesAsync();
            return Ok(new { message = "Заказ архивирован", order });
        }
    }
}