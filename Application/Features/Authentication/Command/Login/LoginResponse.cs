namespace LuckyExpenses.Application.Features.Authentication.Command.Login
{
    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime RefreshTokenExpiresAt { get; set; }
    }
}
