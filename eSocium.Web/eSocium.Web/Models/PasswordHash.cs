using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace eSocium.Web.Models
{
    /// <summary>
    /// Статический класс, который реализует шифрование паролей.
    /// Используется для того, чтобы хранить пароли пользователей в зашифрованном виде
    /// </summary>
    public static class PasswordHash
    {
        private static string goodSymbols = @"[\w\d\!\@\#\$\%\^|&\*\?\+\=\-]*";

        private static HashAlgorithm hash = new System.Security.Cryptography.SHA512Managed();

        /// <summary>
        /// Возвращает истину, если пароль состоит из допустимых символов
        /// </summary>
        /// <param name="pwd">Пароль</param>
        /// <returns>true\false</returns>
        public static bool IsValid(string pwd)
        {
            if (String.IsNullOrEmpty(pwd)) return false;

            Regex regex = new Regex(goodSymbols);
            Match matches = regex.Match(pwd);

            if (matches.Value == pwd)
                return true;
            else
                return false;
        }

        public static string ComputeHash(string pwd)
        {
            byte[] salt = new byte[new Random().Next(4, 8)];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);
            return ComputeHash(pwd, salt);
        }

        public static string ComputeHash(string pwd, byte[] salt)
        {
            if (IsValid(pwd))
            {

                byte[] password = Encoding.UTF8.GetBytes(pwd);
                byte[] passwordAndSaltBytes = new byte[password.Length + salt.Length];
                Array.Copy(password, passwordAndSaltBytes, password.Length);
                Array.Copy(salt, 0, passwordAndSaltBytes, password.Length, salt.Length);


                byte[] hashBytes = hash.ComputeHash(passwordAndSaltBytes);
                byte[] hashBytesWithSalt = new byte[hashBytes.Length + salt.Length];

                Array.Copy(hashBytes, hashBytesWithSalt, hashBytes.Length);
                Array.Copy(salt, 0, hashBytesWithSalt, hashBytes.Length, salt.Length);


                return Convert.ToBase64String(hashBytesWithSalt);
            }
            else
            {
                return "";
            }
        }

        public static bool VerifyHash(string pwd, string passwordHash)
        {
            byte[] pwdHash = Convert.FromBase64String(passwordHash);
            int hashSize = 64;
            if (hashSize > pwdHash.Length) return false;
            byte[] saltBytes = new byte[pwdHash.Length - hashSize];

            for (int i = 0; i < saltBytes.Length; i++)
                saltBytes[i] = pwdHash[hashSize + i];

            string expectedHashString = ComputeHash(pwd, saltBytes);
            int ln = expectedHashString.Length;

            return (passwordHash == expectedHashString);

        }
    }
}