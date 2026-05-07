using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionBatchesController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public ProductionBatchesController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionBatch>>> GetBatches()
        {
            return await _context.ProductionBatches.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionBatch>> GetBatch(int id)
        {
            var batch = await _context.ProductionBatches.FindAsync(id);
            if (batch == null)
                return NotFound(new { message = $"Партия с id {id} не найдена" });
            return batch;
        }

        [HttpGet("number/{batchNumber}")]
        public async Task<ActionResult<ProductionBatch>> GetBatchByNumber(string batchNumber)
        {
            var batch = await _context.ProductionBatches.FirstOrDefaultAsync(b => b.BatchNumber == batchNumber);
            if (batch == null)
                return NotFound(new { message = $"Партия {batchNumber} не найдена" });
            return batch;
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<ProductionBatch>>> GetBatchesByStatus(string status)
        {
            var batches = await _context.ProductionBatches.Where(b => b.Status == status).ToListAsync();
            return batches;
        }

        [HttpGet("order/{orderId}")]
        public async Task<ActionResult<IEnumerable<ProductionBatch>>> GetBatchesByOrder(int orderId)
        {
            var batches = await _context.ProductionBatches.Where(b => b.OrderId == orderId).ToListAsync();
            return batches;
        }

        [HttpPost]
        public async Task<ActionResult<ProductionBatch>> CreateBatch(ProductionBatch batch)
        {
            batch.Status = "planned";
            _context.ProductionBatches.Add(batch);
            await _context.SaveChangesAsync();

            // Автоматически создаем шаги для партии
            await CreateStepsForBatch(batch.Id, batch.OrderId);

            return CreatedAtAction(nameof(GetBatch), new { id = batch.Id }, batch);
        }

        private async Task CreateStepsForBatch(int batchId, int? orderId)
        {
            // Здесь логика создания шагов на основе технологической карты
            var steps = new List<ProductionStep>
            {
                new ProductionStep { BatchId = batchId, StepOrder = 1, StepName = "Смешивание", PlannedTempC = 45, PlannedDurationMin = 30, PlannedPressureBar = 1.5m, StartedAt = DateTime.Now },
                new ProductionStep { BatchId = batchId, StepOrder = 2, StepName = "Выдержка", PlannedTempC = 60, PlannedDurationMin = 120, PlannedPressureBar = 2.0m },
                new ProductionStep { BatchId = batchId, StepOrder = 3, StepName = "Экструзия", PlannedTempC = 80, PlannedDurationMin = 45, PlannedPressureBar = 3.0m },
                new ProductionStep { BatchId = batchId, StepOrder = 4, StepName = "Охлаждение", PlannedTempC = 25, PlannedDurationMin = 60, PlannedPressureBar = 1.0m }
            };
            await _context.ProductionSteps.AddRangeAsync(steps);
            await _context.SaveChangesAsync();
        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartBatch(int id)
        {
            var batch = await _context.ProductionBatches.FindAsync(id);
            if (batch == null)
                return NotFound(new { message = $"Партия с id {id} не найдена" });

            batch.Status = "running";
            batch.StartTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Партия запущена", batch });
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteBatch(int id)
        {
            var batch = await _context.ProductionBatches.FindAsync(id);
            if (batch == null)
                return NotFound(new { message = $"Партия с id {id} не найдена" });

            batch.Status = "quality_pending";
            batch.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Партия завершена, ожидает контроля качества", batch });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBatch(int id, ProductionBatch batch)
        {
            if (id != batch.Id)
                return BadRequest(new { message = "ID не совпадают" });

            var existingBatch = await _context.ProductionBatches.FindAsync(id);
            if (existingBatch == null)
                return NotFound(new { message = $"Партия с id {id} не найдена" });

            existingBatch.ActualQuantityKg = batch.ActualQuantityKg;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}