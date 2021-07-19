using System;
using System.Security.Cryptography;

namespace Campton
{
    public class ProfileCondition
    {
        public readonly Random Random = new();
        private const string WordDictionary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        
        /// <summary>
        /// Создание хеша по определенной длине для пользователя
        /// </summary>
        /// <param name="lengthHash"></param>
        /// <returns></returns>
        public string CreateHash(int lengthHash)
        {
            var hash = "";
            for (var i = 0; i < lengthHash; i++)
                hash += WordDictionary[Random.Next(WordDictionary.Length)];
            
            return hash;
        }
        /// <summary>
        /// Хеширование пароля для пользователя
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                return "";
            }
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        /// <summary>
        /// Проверка хешированого пароля
        /// </summary>
        /// <param name="hashedPassword"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer;

            if (hashedPassword == null || password == null)
                return false;

            var src = Convert.FromBase64String(hashedPassword);
            if (src.Length != 0x31 || src[0] != 0)
                return false;

            var dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            var hashedBuffer = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, hashedBuffer, 0, 0x20);

            using (var bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
                buffer = bytes.GetBytes(0x20);

            var result = ByteArraysEqual(ref hashedBuffer, ref buffer);
            return result;
        }
        private bool ByteArraysEqual(ref byte[] b1, ref byte[] b2)
        {
            if (b1 == b2)
                return true;
            if (b1 == null || b2 == null)
                return false;
            if (b1.Length != b2.Length)
                return false;
            for (var i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                    return false;
            }
            return true;
        }
    }
}
