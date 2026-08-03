using LuckyExpenses.Application.Interfaces.Authentication;
using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;

namespace LuckyExpenses.Infrastructure.Authentication
{
    public class HasherService : IHasherService
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int DegreeOfParallelism = 4;
        private const int MemorySize = 65536;
        private const int Iterations = 3;

        public string Hash(string input)
        {
            var salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);

            var hashBytes = ComputeHashBytes(input, salt);

            return FormatPhc(salt, hashBytes);
        }

        public bool Verify(string input, string existingHash)
        {
            var parameters = ParsePhc(existingHash);

            var computed = ComputeHashBytes(input, parameters.Salt);
            var stored = Convert.FromBase64String(parameters.Hash);

            return CryptographicOperations.FixedTimeEquals(computed, stored);
        }

        private static byte[] ComputeHashBytes(string input, byte[] salt)
        {
            var argon2 = new Argon2id(Encoding.UTF8.GetBytes(input))
            {
                Salt = salt,
                DegreeOfParallelism = DegreeOfParallelism,
                MemorySize = MemorySize,
                Iterations = Iterations
            };

            return argon2.GetBytes(HashSize);
        }

        private static string FormatPhc(byte[] salt, byte[] hash)
        {
            return $"$argon2id$v=19$m={MemorySize},t={Iterations},p={DegreeOfParallelism}$" +
                   $"{Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        private static (byte[] Salt, string Hash) ParsePhc(string storedHash)
        {
            var parts = storedHash.Split('$');
            if (parts.Length < 5)
                throw new FormatException("Hash en formato PHC inválido");

            return (Convert.FromBase64String(parts[^2]), parts[^1]);
        }
    }
}