namespace PulsePoint.Models.DTOs
{
    public class RegisterDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int WorkplaceId { get; set; }
    }
}
