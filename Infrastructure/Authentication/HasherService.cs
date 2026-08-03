using LuckyExpenses.Domain.Services;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace LuckyExpenses.Infrastructure.Authentication
{
    public class HasherService : IHasherService
    {
        public byte[] ComputeHashBytes(string input, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(input))
            {
                Salt = salt,
                DegreeOfParallelism = 4,
                MemorySize = 65536,
                Iterations = 3
            };
            return argon2.GetBytes(32);
        }

        public string ComputeHash(string input, byte[] salt)
        {
            return Convert.ToBase64String(ComputeHashBytes(input, salt));
        }

        public bool Verify(string input, string existingHash, byte[] salt)
        {
            var computed = ComputeHashBytes(input, salt);
            var existing = Convert.FromBase64String(existingHash);
            return CryptographicOperations.FixedTimeEquals(computed, existing);
        }

        public byte[] GenerateSalt(int size = 16)
        {
            var salt = new byte[size];
            RandomNumberGenerator.Fill(salt);
            return salt;
        }
    }
}