using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model.DTO
{
    public class SMSDTO
    {
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Content { get; set; }
        public string UpdateTime { get; set; }
        public bool Status { get; set; }
    }
}
