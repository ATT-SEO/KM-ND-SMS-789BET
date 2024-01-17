namespace FE.ADMIN.Utility
{
	public class CoreBase
	{
		public static int CalculateTotalPages(int totalCount, int pageSize)
		{
			return (int)Math.Ceiling(totalCount / (double)pageSize);
		}
	}
}
