using LuckyExpenses.Domain.Common;
using LuckyExpenses.Domain.Enums;

namespace LuckyExpenses.Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRoleEnum Role { get; set; } = UserRoleEnum.USER;

        public UserStateEnum State { get; set; } = UserStateEnum.ACTIVE;
    }
}
