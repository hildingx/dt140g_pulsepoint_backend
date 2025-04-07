namespace PulsePoint.Models
{
    public class HealthEntry
    {
        public int Id { get; set; }

        public int Mood { get; set; }
        public int Sleep { get; set; }
        public int Stress { get; set; }
        public int Activity { get; set; }
        public int Nutrition { get; set; }

        public DateTime Date { get; set; }

        public int UserId { get; set; } // FK till User
        public User User { get; set; } = null!; // Navigation property
    }
}
