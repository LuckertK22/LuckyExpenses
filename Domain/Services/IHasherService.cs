namespace LuckyExpenses.Domain.Services
{
    public interface IHasherService
    {
        byte[] ComputeHashBytes(string input, byte[] salt);
        string ComputeHash(string input, byte[] salt);
        bool Verify(string input, string existingHash, byte[] salt);
        byte[] GenerateSalt(int size = 16);
    }
}