using System.Text.RegularExpressions;

namespace FE.ADMIN.Utility
{
	public class ConverPhoneShow
	{
		public static string FormatPhoneNumber(string phoneNumber)
		{
			string cleanedPhoneNumber = Regex.Replace(phoneNumber, @"\s+", "");
			if (cleanedPhoneNumber.Length >= 10)
			{
				string prefix = cleanedPhoneNumber.Substring(0, 4);
				string suffix = cleanedPhoneNumber.Substring(cleanedPhoneNumber.Length - 3);

				cleanedPhoneNumber = $"{prefix}xxxxx{suffix}";
				return cleanedPhoneNumber;
			}
			else
			{
				return "xxxxxxxxxx";
			}
		}

	}

}
