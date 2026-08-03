namespace LuckyExpenses.Domain.Services
{
    public interface ITokenService
    {
        string GenerateToken(long userId, string email, string role);
    }
}