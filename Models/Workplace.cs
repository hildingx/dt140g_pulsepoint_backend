namespace PulsePoint.Models
{
    /// <summary>
    /// Representerar en arbetsplats i systemet. 
    /// En arbetsplats kan ha flera användare kopplade till sig.
    /// </summary>
    public class Workplace
    {
        /// <summary>
        /// Unikt ID för arbetsplatsen (primärnyckel i databasen).
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Namn på arbetsplatsen (obligatoriskt fält).
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Navigation property – lista över användare som tillhör denna arbetsplats.
        /// </summary>
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
