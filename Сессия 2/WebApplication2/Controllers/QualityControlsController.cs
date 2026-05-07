using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QualityControlsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public QualityControlsController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QualityControl>>> GetQualityControls()
        {
            return await _context.QualityControls.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QualityControl>> GetQualityControl(int id)
        {
            var qc = await _context.QualityControls.FindAsync(id);
            if (qc == null)
                return NotFound(new { message = $"Анализ с id {id} не найден" });
            return qc;
        }

        [HttpGet("batch/{batchId}")]
        public async Task<ActionResult<IEnumerable<QualityControl>>> GetByBatch(int batchId)
        {
            var qcs = await _context.QualityControls.Where(q => q.BatchId == batchId).ToListAsync();
            return qcs;
        }

        [HttpGet("batch/{batchId}/pending")]
        public async Task<ActionResult<IEnumerable<QualityControl>>> GetPendingByBatch(int batchId)
        {
            var pending = await _context.QualityControls.Where(q => q.BatchId == batchId && q.Decision == "pending").ToListAsync();
            return pending;
        }

        [HttpGet("decision/{decision}")]
        public async Task<ActionResult<IEnumerable<QualityControl>>> GetByDecision(string decision)
        {
            var qcs = await _context.QualityControls.Where(q => q.Decision == decision).ToListAsync();
            return qcs;
        }

        [HttpPost]
        public async Task<ActionResult<QualityControl>> CreateQualityControl(QualityControl qc)
        {
            qc.AnalysisDate = DateTime.Now;
            qc.Decision = "pending";
            _context.QualityControls.Add(qc);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetQualityControl), new { id = qc.Id }, qc);
        }

        [HttpPut("{id}/decision")]
        public async Task<IActionResult> SetDecision(int id, [FromBody] DecisionRequest request)
        {
            var qc = await _context.QualityControls.FindAsync(id);
            if (qc == null)
                return NotFound(new { message = $"Анализ с id {id} не найден" });

            qc.Decision = request.Decision;
            qc.AnalystComment = request.Comment;
            await _context.SaveChangesAsync();

            // Если это финальное решение для партии
            var batch = await _context.ProductionBatches.FindAsync(qc.BatchId);
            if (batch != null && batch.Status == "quality_pending")
            {
                batch.Status = request.Decision == "approved" ? "completed" : "blocked";
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = $"Решение '{request.Decision}' принято", qc });
        }
    }

    public class DecisionRequest
    {
        public string Decision { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}