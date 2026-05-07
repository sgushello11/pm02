namespace WebApplication2.Models
{
    public class RecipeComponent
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int MaterialId { get; set; }
        public decimal Percentage { get; set; }
        public decimal Tolerance { get; set; }
        public int OrderIndex { get; set; }
    }
}
