using System;

namespace WebApplication2.Models
{
    public class TechMap
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Version { get; set; } = 1;
        public string Status { get; set; } = "draft";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? ApprovedAt { get; set; }
        public int? ApprovedBy { get; set; }
    }
}