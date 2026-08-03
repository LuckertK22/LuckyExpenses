namespace LuckyExpenses.Domain.Exceptions
{
    public class ConflictException(string message, Exception? innerException = null)
        : ApplicationException(message, innerException)
    {
    }
}
