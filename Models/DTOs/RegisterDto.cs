namespace PulsePoint.Models.DTOs
{
    /// <summary>
    /// DTO för registrering. Används vid skapande av nya användare via /api/auth/register.
    /// </summary>
    public class RegisterDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int WorkplaceId { get; set; }
    }
}
