namespace LuckyExpenses.Domain.Exceptions
{
    public class NotFoundException(string message) : ApplicationException(message)
    {
    }
}
