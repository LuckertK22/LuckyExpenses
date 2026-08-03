using Microsoft.AspNetCore.Identity;

namespace LuckyExpenses.Infrastructure.Persistence
{
    public class ApplicationUser : IdentityUser<long>
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
    }
}