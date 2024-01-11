using System.Text.RegularExpressions;

namespace FE.CLIENT.Utility
{
    public class ConvertPhoneNumber
    {
        public static string ConvertPhone(string phoneNumber)
        {
            string cleanedPhoneNumber = Regex.Replace(phoneNumber, @"\s+", "");
            if (cleanedPhoneNumber.StartsWith("84"))
            {
                return "+" + cleanedPhoneNumber;
            }
            else if (cleanedPhoneNumber.StartsWith("0"))
            {
                return cleanedPhoneNumber;
            }
            else
            {
                return "0" + cleanedPhoneNumber;
            }
        }
    }
}
