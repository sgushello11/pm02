using System;

namespace WebApplication2.Models
{
    public class ProductionStep
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int StepOrder { get; set; }
        public string StepName { get; set; } = string.Empty;
        public decimal? PlannedTempC { get; set; }
        public decimal? ActualTempC { get; set; }
        public int? PlannedDurationMin { get; set; }
        public int? ActualDurationMin { get; set; }
        public decimal? PlannedPressureBar { get; set; }
        public decimal? ActualPressureBar { get; set; }
        public bool DeviationFlag { get; set; } = false;
        public string OperatorComment { get; set; } = string.Empty;
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
    }
}