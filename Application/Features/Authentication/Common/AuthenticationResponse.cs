namespace LuckyExpenses.Application.Features.Authentication.Common
{
    public sealed record AuthenticationResponse(
        string Token,
        string Email,
        string Role,
        DateTime ExpiresAt);
}