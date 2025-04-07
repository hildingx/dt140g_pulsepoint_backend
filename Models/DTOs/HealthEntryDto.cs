namespace PulsePoint.Models.DTOs
{
    public class HealthEntryDto
    {
        public int Mood { get; set; }
        public int Sleep { get; set; }
        public int Stress { get; set; }
        public int Activity { get; set; }
        public int Nutrition { get; set; }
        public int UserId { get; set; }
    }
}
