namespace FE.ADMIN.Models
{
    public class SMSDTO
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public string UpdateTime { get; set; }
        public string? Device { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
