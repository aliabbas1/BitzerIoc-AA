using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text;

namespace BitzerIoC.Infrastructure.Security
{
    /// <summary>
    /// No inhertience allowed
    /// </summary>
    public sealed class HashSecurity
    {
        /// <summary>
        /// Salt byte size
        /// </summary>
        private static readonly int saltSize = 128/8;

        /// <summary>
        /// Get the hash od provided text
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public static string GetHash(string password)
        {
           using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
               
                // Get the hashed string.  
                 return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
              
            }
        }

        /// <summary>
        /// Get the hash od provided text
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GetHash(string password,string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                // Send a sample text to hash.  
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password+salt));

                // Get the hashed string.  
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            }
        }

        /// <summary>
        /// Get salt of 128/8 byte size, taken from class property
        /// </summary>
        /// <returns></returns>
        public static string GetSalt()
        {
            byte[] bytes = new byte[saltSize];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }


        /// <summary>
        /// Compare password hash and return true or false
        /// </summary>
        /// <param name="inputText">Text which need to be compared</param>
        /// <param name="compareWithText">Text with which input text need to be compared</param>
        /// <param name="secretHashSalt">Secret Salt, stored for a particualr text sequence</param>
        /// <returns></returns>
        public static bool CompareHashText(string inputText,string compareWithText ,string secretHashSalt)
        {
            string hashedPassword = GetHash(inputText + secretHashSalt);
            if (hashedPassword.Equals(compareWithText))
            {
                return true;
            }
            return false;
        }

       

    }
}


///// <summary>
///// 
///// </summary>
///// <param name="password"></param>
///// <param name="salt"></param>
///// <param name="byteRequested">Max size: 256/8</param>
///// <param name="iterationCount">10000</param>
///// <param name="pref">KeyDerivationPrf.HMACSHA1</param>
///// <returns></returns>
//public static string Hash(string password, byte[] salt, int byteRequested = 256 / 8, int iterationCount = 1000, KeyDerivationPrf pref = KeyDerivationPrf.HMACSHA1)
//{
//    return Convert.ToBase64String(KeyDerivation.Pbkdf2(
//                    password: password,
//                    salt: salt,
//                    prf: pref,
//                    iterationCount: iterationCount,
//                    numBytesRequested: byteRequested));
//}


///// <summary>
///// Get the generated salt
///// </summary>
///// <param name="size">128/8</param>
///// <returns></returns>
//public static byte[] GetSalt(int size)
//{
//    byte[] salt = new byte[size];
//    using (var rng = RandomNumberGenerator.Create())
//    {
//        rng.GetBytes(salt);
//    }
//    return salt;
//}