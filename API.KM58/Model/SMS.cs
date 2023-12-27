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
        public string UpdateTime { get; set; }
        public bool Status { get; set; }
    }
}
