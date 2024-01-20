namespace FE.ADMIN.Utility
{
	public class CoreBase
	{
		public static int CalculateTotalPages(int totalCount, int pageSize)
		{
			return (int)Math.Ceiling(totalCount / (double)pageSize);
		}
		public static string ToQueryString(object obj)
		{
			var properties = obj.GetType().GetProperties();
			var keyValuePairs = properties
				.Where(property => property.GetValue(obj) != null)
				.Select(property => $"{property.Name}={Uri.EscapeDataString(property.GetValue(obj).ToString())}");

			return string.Join("&", keyValuePairs);
		}
	}
}
