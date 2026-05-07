using System;

namespace WebApplication2.Models
{
    public class Deviation
    {
        public int Id { get; set; }
        public int BatchId { get; set; }
        public int? StepId { get; set; }
        public string DeviationType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = "low"; // low, medium, high, critical
        public int? ReportedBy { get; set; }
        public DateTime ReportedAt { get; set; } = DateTime.Now;
        public DateTime? ResolvedAt { get; set; }
        public string ResolutionComment { get; set; } = string.Empty;
    }
}