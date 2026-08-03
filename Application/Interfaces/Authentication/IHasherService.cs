namespace LuckyExpenses.Application.Interfaces.Authentication
{
    public interface IHasherService
    {
        string Hash(string input);
        bool Verify(string input, string existingHash);
    }
}
