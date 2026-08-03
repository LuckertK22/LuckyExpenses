namespace LuckyExpenses.Application.Features.Authentication.Command.Login
{
    public sealed record LoginResponse(
        string Token,
        string Email,
        string Role,
        DateTime ExpiresAt);
}
