using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductionStepsController : ControllerBase
    {
        private readonly AgroControlDbContext _context;

        public ProductionStepsController(AgroControlDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductionStep>>> GetSteps()
        {
            return await _context.ProductionSteps.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductionStep>> GetStep(int id)
        {
            var step = await _context.ProductionSteps.FindAsync(id);
            if (step == null)
                return NotFound(new { message = $"Шаг с id {id} не найден" });
            return step;
        }

        [HttpGet("batch/{batchId}")]
        public async Task<ActionResult<IEnumerable<ProductionStep>>> GetStepsByBatch(int batchId)
        {
            var steps = await _context.ProductionSteps.Where(s => s.BatchId == batchId).OrderBy(s => s.StepOrder).ToListAsync();
            return steps;
        }

        [HttpGet("batch/{batchId}/current")]
        public async Task<ActionResult<ProductionStep>> GetCurrentStep(int batchId)
        {
            var currentStep = await _context.ProductionSteps.FirstOrDefaultAsync(s => s.BatchId == batchId && s.StartedAt != null && s.CompletedAt == null);
            if (currentStep == null)
                return NotFound(new { message = "Нет активного шага для этой партии" });
            return currentStep;
        }

        [HttpPut("{id}/start")]
        public async Task<IActionResult> StartStep(int id)
        {
            var step = await _context.ProductionSteps.FindAsync(id);
            if (step == null)
                return NotFound(new { message = $"Шаг с id {id} не найден" });

            step.StartedAt = DateTime.Now;
            await _context.SaveChangesAsync();
            return Ok(new { message = "Шаг начат", step });
        }

        [HttpPut("{id}/complete")]
        public async Task<IActionResult> CompleteStep(int id, [FromBody] StepCompletionRequest request)
        {
            var step = await _context.ProductionSteps.FindAsync(id);
            if (step == null)
                return NotFound(new { message = $"Шаг с id {id} не найден" });

            step.ActualTempC = request.ActualTempC;
            step.ActualDurationMin = request.ActualDurationMin;
            step.ActualPressureBar = request.ActualPressureBar;
            step.OperatorComment = request.OperatorComment;
            step.CompletedAt = DateTime.Now;

            // Проверка отклонений
            if ((step.PlannedTempC.HasValue && Math.Abs((step.ActualTempC ?? 0) - step.PlannedTempC.Value) > 5) ||
                (step.PlannedPressureBar.HasValue && Math.Abs((step.ActualPressureBar ?? 0) - step.PlannedPressureBar.Value) > 0.5m))
            {
                step.DeviationFlag = true;

                // Создаем запись об отклонении
                var deviation = new Deviation
                {
                    BatchId = step.BatchId,
                    StepId = step.Id,
                    DeviationType = "Параметрическое отклонение",
                    Description = $"Отклонение по параметрам: T={step.ActualTempC}°C (норма {step.PlannedTempC}°C), P={step.ActualPressureBar} бар (норма {step.PlannedPressureBar} бар)",
                    Severity = "medium",
                    ReportedAt = DateTime.Now
                };
                _context.Deviations.Add(deviation);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Шаг завершен", step });
        }
    }

    public class StepCompletionRequest
    {
        public decimal? ActualTempC { get; set; }
        public int? ActualDurationMin { get; set; }
        public decimal? ActualPressureBar { get; set; }
        public string OperatorComment { get; set; } = string.Empty;
    }
}