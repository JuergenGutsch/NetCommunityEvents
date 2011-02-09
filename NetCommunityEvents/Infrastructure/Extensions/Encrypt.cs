using System;
using System.Security.Cryptography;
using System.Text;

namespace NetCommunityEvents.Infrastructure.Extensions
{
    /// <summary>
    /// Summary description for Encrypt
    /// </summary>
    public static class Encrypt
    {

        private const string UpperAlphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string LowerAlphabets = "abcdefghijklmnopqrstuvwxyz";
        private const string Numbers = "01234567890123456789012345";

        // -----------------------------------------------------------------------------
        // <summary>
        // Ermittelt den Hashwert eines �bergebenen Strings
        // </summary>
        // <param name="Value">String dessen Hashwert ermittelt werden soll</param>
        // <returns>den HashWert de s�bergebenen Strings. 
        // Einen String wie z. B: 
        // "D4-1D-8C-D9-8F-00-B2-04-E9-80-09-98-EC-F8-42-7E"</returns>
        // -----------------------------------------------------------------------------
        public static string ConvertToHash(this string value)
        {
            Byte[] dataToHash = (new UnicodeEncoding()).GetBytes(value);
            Byte[] hashValue = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(dataToHash);

            return BitConverter.ToString(hashValue);
        }

        public static string CreateNewPassword()
        {
            StringBuilder randomString = new StringBuilder();
            Random random = new Random();
            for (int j = 0; j <= 4; j++)
            {
                randomString.Append(UpperAlphabets[random.Next(UpperAlphabets.Length)]);
                randomString.Append(LowerAlphabets[random.Next(LowerAlphabets.Length)]);
                randomString.Append(Numbers[random.Next(Numbers.Length)]);
            }
            return randomString.ToString();
        }

    }
}