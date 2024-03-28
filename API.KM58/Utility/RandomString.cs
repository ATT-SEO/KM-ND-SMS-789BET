using System.Text.RegularExpressions;
using System.Text;
using API.KM58.Model.DTO;
using System.Security.Cryptography;

namespace API.KM58.Utility
{
    public class RandomString
    {
        public static string GenerateString(int total = 9, int text1 = 3, int number = 3, int text2 = 3)
        {
            int remainingChars = total;

            int text1Chars = Math.Min(text1, remainingChars);
            string result = GenerateRandomLetters(text1Chars);
            remainingChars -= text1Chars;

            int numberChars = Math.Min(number, remainingChars);
            result += GenerateRandomNumbers(numberChars);
            remainingChars -= numberChars;

            int text2Chars = Math.Min(text2, remainingChars);
            result += GenerateRandomLetters(text2Chars);
            return result;
        }

        private static string GenerateRandomLetters(int count)
        {
            Random random = new Random();
            string letters = "ABCDEFGHJKMNOPQRSTUVWXYZ";
            letters += letters.ToLower();
            return new string(Enumerable.Repeat(letters, count)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string GenerateRandomNumbers(int count)
        {
            Random random = new Random();
            string numbers = "0123456789";
            return new string(Enumerable.Repeat(numbers, count)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string ComputeSHA1Hash(string combinedString)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] hashBytes = sha1.ComputeHash(Encoding.UTF8.GetBytes(combinedString));

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
