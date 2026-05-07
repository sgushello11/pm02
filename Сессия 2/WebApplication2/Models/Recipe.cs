namespace WebApplication2.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Version { get; set; } = 1;
        public string Status { get; set; } = "draft"; // draft, active, archived
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? ProductId { get; set; }
    }
}