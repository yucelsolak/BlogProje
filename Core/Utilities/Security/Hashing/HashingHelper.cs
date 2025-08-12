using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Hasing
{
    public static class HashingHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password is required.", nameof(password));

            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key; // 128 byte
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)); // 64 byte
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password) || passwordHash is null || passwordSalt is null)
                return false;

            using var hmac = new HMACSHA512(passwordSalt);
            var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return CryptographicOperations.FixedTimeEquals(computed, passwordHash);
        }
    }
}
