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

        public int Mood { get; set; }          // Hur användaren mår (ex. 1–5)
        public int Sleep { get; set; }         // Sömnkvalitet (ex. 1–5)
        public int Stress { get; set; }        // Stressnivå (ex. 1–5)
        public int Activity { get; set; }      // Fysisk aktivitet (ex. 1–5)
        public int Nutrition { get; set; }     // Kosthållning (ex. 1–5)

        public DateTime Date { get; set; }     // Datum för inmatningen (sätts automatiskt)

        public int UserId { get; set; }        // Foreign key till användare

        /// <summary>
        /// Navigation property – den användare som äger denna health entry.
        /// </summary>
        public User User { get; set; } = null!;
    }
}
