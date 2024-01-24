namespace FE.ADMIN.Models
{
    public class QueryParametersDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 100;
        public string? IP { get; set; }
        public string? FP { get; set; }
        public int? SiteID { get; set; }
        public string? Account { get; set; }
        public string? Sender { get; set; }
        public string? Content { get; set; }
        public string? Device { get; set; }
		public string? ProjectCode { get; set; }
		public int SearchStatus {get; set; }
		public string? SortBy { get; set; }
        public string? SortDirection { get; set; }
    }
}
