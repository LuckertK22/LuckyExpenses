namespace LuckyExpenses.Domain.Exceptions
{
    public class UserInactiveException(string message) : ApplicationException(message)
    {
    }
}
