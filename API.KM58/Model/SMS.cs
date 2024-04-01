using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model
{
    public class SMS
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Sender { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public bool Status { get; set; }
        public bool AutoPoint { get; set; } = false;
        public string? Account { get; set; }
        public int Point { get; set; }
        public string? Device { get; set; }
        public string? PhoneReceive { get; set; }
        public string? ProjectCode { get; set; }
        public string? IP { get; set; }
        public string? FP { get; set; }
		public string? AgentText { get; set; }
		public DateTime? CreatedTime { get; set; }
        public DateTime? SiteTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
