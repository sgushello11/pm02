using System;

namespace WebApplication2.Models
{
    public class ProductionBatch
    {
        public int Id { get; set; }
        public string BatchNumber { get; set; } = string.Empty;
        public int? OrderId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; } = "planned"; // planned, running, completed, blocked, quality_pending
        public decimal ActualQuantityKg { get; set; } = 0;
    }
}