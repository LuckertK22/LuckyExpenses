namespace LuckyExpenses.Domain.Exceptions
{
    public class InvalidCredentialsException(string message) : ApplicationException(message)
    {
    }
}
