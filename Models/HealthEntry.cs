using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PulsePoint.Models
{
    /// <summary>
    /// Representerar en hälsoregistrering som en användare skickat in.
    /// Innehåller värden för olika hälsoparametrar, datum och referens till användaren.
    /// </summary>
    public class HealthEntry
    {
        public int Id { get; set; }

        public int Mood { get; set; }
        public int Sleep { get; set; }
        public int Stress { get; set; }
        public int Activity { get; set; }
        public int Nutrition { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; }        // Foreign key till användare

        /// <summary>
        /// Navigation property – den användare som äger denna health entry.
        /// </summary>
        public User User { get; set; } = null!;
    }
}
