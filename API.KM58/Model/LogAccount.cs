using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KM58.Model
{
	public class LogAccount
	{
		[Key]
		public int Id { get; set; }
		public string? Account { get; set; }
		public string? IP { get; set; }
		public string? FP { get; set; }
        public string? Project { get; set; }
        public string? Token { get; set; }
        public DateTime? CreatedTime { get; set; }
	}
}
