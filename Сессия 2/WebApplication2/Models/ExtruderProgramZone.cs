namespace WebApplication2.Models
{
    public class ExtruderProgramZone
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int ZoneNumber { get; set; }
        public decimal TargetTemperature { get; set; }
        public decimal Tolerance { get; set; }
        public decimal TargetPressure { get; set; }
        public int TargetSpeed { get; set; }
    }
}