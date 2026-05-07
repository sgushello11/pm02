using System;

namespace WebApplication2.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = "kg";
        public decimal? DefaultMinValue { get; set; }
        public decimal? DefaultMaxValue { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsActive { get; set; } = true;
    }
}