namespace LuckyExpenses.Application.Interfaces.Authentication
{
    public interface ITokenService
    {
        string GenerateToken(long userId, string email, string role, DateTime expiration);

        string GenerateRefreshToken();

        string HashRefreshToken(string token);
    }
}