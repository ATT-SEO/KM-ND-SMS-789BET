namespace FE.ADMIN.Models
{
	public class LogAccountDTO
	{
		public int Id { get; set; }
		public string? Account { get; set; }
		public string? IP { get; set; }
		public string? FP { get; set; }
		public int SiteID { get; set; }
		public SiteDTO? Site { get; set; }
		public string? Project { get; set; }
		public DateTime? CreatedTime { get; set; }
	}
}
