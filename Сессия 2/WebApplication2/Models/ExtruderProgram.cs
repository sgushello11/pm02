using System;

namespace WebApplication2.Models
{
    public class ExtruderProgram
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? RecipeId { get; set; }
        public int? EquipmentId { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
    }
}