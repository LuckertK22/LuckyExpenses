namespace LuckyExpenses.Application.Features.Users.Common
{
    public sealed record UserResponse(
        long Id,
        string FirstName,
        string LastName,
        string Email,
        string Role);
}
