using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptoHelper
{
    public class Encrypt
    {
        /// <summary>
        /// Mã hóa chuỗi ký tự
        /// </summary>
        /// <param name="plaintext">chuỗi cần mã hóa</param>
        /// <param name="salt">salt</param>
        /// <returns>Chuỗi đã được mã hóa</returns>
        public static string EncryptPassword(string plaintext, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = string.Format("{0}.{1}", salt, plaintext);
                byte[] saltedPasswordAsBytes = Encoding.UTF8.GetBytes(saltedPassword);
                return Convert.ToBase64String(sha256.ComputeHash(saltedPasswordAsBytes));
            }
        }
    }
}
