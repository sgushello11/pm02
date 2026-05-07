namespace WebApplication2.Models
{
    public class TechMapStep
    {
        public int Id { get; set; }
        public int TechMapId { get; set; }
        public int StepOrder { get; set; }
        public string StepType { get; set; } = string.Empty;
        public string Instruction { get; set; } = string.Empty;
        public int? DurationMin { get; set; }
        public bool IsMandatory { get; set; } = true;
        public int? EquipmentId { get; set; }
    }
}