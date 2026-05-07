using System;

namespace WebApplication2.Models
{
    public class ProductionOrder
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public int? RecipeId { get; set; }
        public decimal PlannedQuantityKg { get; set; }
        public string Status { get; set; } = "planned"; // draft, planned, in_progress, completed, archived
        public DateTime? PlannedStartDate { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}