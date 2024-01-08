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
        public string? Device { get; set; }
        public string? ProjectCode { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? SiteTime { get; set; }
    }
}
