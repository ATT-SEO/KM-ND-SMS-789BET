namespace FE.ADMIN.Models
{
    public class SMSDTO
    {
		public int Id { get; set; }
		public string? Sender { get; set; }
		public string? Content { get; set; }
		public string? Device { get; set; }
		public string? ProjectCode { get; set; }
        public string? Account { get; set; }
        public int Point { get; set; }
        public bool Status { get; set; }
		public DateTime? CreatedTime { get; set; }
		public DateTime? SiteTime { get; set; }

	}
}
