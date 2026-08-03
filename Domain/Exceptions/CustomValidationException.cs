namespace LuckyExpenses.Domain.Exceptions
{
    public class CustomValidationException(Dictionary<string, string[]> errors)
        : ApplicationException("Ha ocurrido uno o mas errores de validacion")
    {
        public Dictionary<string, string[]> Errors { get; } = errors;
    }
}
