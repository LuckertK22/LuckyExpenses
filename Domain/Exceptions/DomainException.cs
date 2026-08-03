namespace LuckyExpenses.Domain.Exceptions
{
    public class DomainException(string message) : ApplicationException(message)
    {
    }
}