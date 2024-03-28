using System.Text.RegularExpressions;

namespace API.KM58.Utility
{
    public class ConvertPhoneNumber
    {
        public static string ConvertPhone(string phoneNumber)
        {
            string cleanedPhoneNumber = Regex.Replace(phoneNumber, @"\s+", "");
            if (cleanedPhoneNumber.StartsWith("0"))
            {
                return "+84" + cleanedPhoneNumber.Substring(1);
            }
            else if (cleanedPhoneNumber.StartsWith("+"))
            {
                return cleanedPhoneNumber;
            }
            else if (cleanedPhoneNumber.StartsWith("84"))
            {
                return "+" + cleanedPhoneNumber;
            }
            else
            {
                return "+84" + cleanedPhoneNumber;
            }
        }
    }
}
