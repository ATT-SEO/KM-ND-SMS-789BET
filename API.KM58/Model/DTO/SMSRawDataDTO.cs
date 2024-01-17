using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model
{
    public class SMSRawDataDTO
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public string Device { get; set; }
        public string ProjectID { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedTime { get; set; }
    }
}
