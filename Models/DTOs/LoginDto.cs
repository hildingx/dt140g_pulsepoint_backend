namespace PulsePoint.Models.DTOs
{
    /// <summary>
    /// DTO för inloggning. Används när användaren loggar in via /api/auth/login.
    /// </summary>
    public class LoginDto
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
