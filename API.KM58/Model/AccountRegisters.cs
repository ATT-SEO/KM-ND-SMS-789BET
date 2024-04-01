using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model
{
    public class AccountRegisters
    {
        [Key]
        public int Id { get; set; }
        public string? Sender { get; set; }
        public string? Content { get; set; }
        public int Status { get; set; }
        public bool AutoPoint { get; set; } = false;
        public bool isSMS { get; set; }
        public string? Account { get; set; }
        public int Point { get; set; }
        public int Audit { get; set; }
        public string? Device { get; set; }
        public string? ProjectCode { get; set; }
        public string? UserPoint { get; set; }
        public string? IP { get; set; }
        public string? FP { get; set; }
        public string? reason_deny { get; set; } = null;
        public string? handler { get; set; }
        public string? Token { get; set; }
		public string? AgentText { get; set; }
		public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }

    }
}
