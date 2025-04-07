using Microsoft.AspNetCore.Identity;

namespace PulsePoint.Models
{
    public class User : IdentityUser<int>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int WorkplaceId { get; set; } // FK till Workplace
        public Workplace Workplace { get; set; } = null!; // Navigation property

        public ICollection<HealthEntry> HealthEntries { get; set; } = new List<HealthEntry>(); // Navigation property
    }
}
