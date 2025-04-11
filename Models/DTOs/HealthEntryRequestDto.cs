namespace PulsePoint.Models.DTOs
{
    /// <summary>
    /// DTO för att skapa eller uppdatera en health entry.
    /// Används vid POST och PUT till /api/healthentries.
    /// </summary>
    public class HealthEntryRequestDto
    {
        public int Mood { get; set; }
        public int Sleep { get; set; }
        public int Stress { get; set; }
        public int Activity { get; set; }
        public int Nutrition { get; set; }
    }
}
