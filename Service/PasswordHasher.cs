using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PasswordHasher
    {
        // Mã hóa password bằng SHA256
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        // So sánh password thô với password đã mã hóa trong DB
        public static bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            var hashOfInput = HashPassword(inputPassword);
            return hashOfInput == hashedPassword;
        }
    }
}
