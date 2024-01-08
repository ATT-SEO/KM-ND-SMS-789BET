using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model.DTO
{
    public class SMSDTO
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public string? Device { get; set; }
        public string? ProjectCode { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
