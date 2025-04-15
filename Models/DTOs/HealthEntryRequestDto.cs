using System.ComponentModel.DataAnnotations;

namespace PulsePoint.Models.DTOs
{
    /// <summary>
    /// DTO för att skapa eller uppdatera en health entry.
    /// Används vid POST och PUT till /api/healthentries.
    /// </summary>
    public class HealthEntryRequestDto
    {
        [Range(1, 5)]
        public int Mood { get; set; }

        [Range(1, 5)]
        public int Sleep { get; set; }

        [Range(1, 5)]
        public int Stress { get; set; }

        [Range(1, 5)]
        public int Activity { get; set; }

        [Range(1, 5)]
        public int Nutrition { get; set; }
    }
}
