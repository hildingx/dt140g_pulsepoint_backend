using Microsoft.AspNetCore.Identity;

namespace PulsePoint.Models
{
    /// <summary>
    /// Utökad Identity-användare med koppling till arbetsplats och hälsodata.
    /// Innehåller grundläggande information samt navigation till relaterade entiteter.
    /// </summary>
    public class User : IdentityUser<int> // Ärver standardfält som Id, UserName, PasswordHash m.m.
    {
        public string? FirstName { get; set; } // Förnamn (frivilligt)
        public string? LastName { get; set; }  // Efternamn (frivilligt)

        public int WorkplaceId { get; set; } // Foreign key till Workplace

        /// <summary>
        /// Navigation property – arbetsplatsen användaren tillhör.
        /// </summary>
        public Workplace Workplace { get; set; } = null!;

        /// <summary>
        /// Navigation property – alla hälsoregistreringar som användaren gjort.
        /// </summary>
        public ICollection<HealthEntry> HealthEntries { get; set; } = new List<HealthEntry>();
    }
}
