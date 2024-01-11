using System.ComponentModel.DataAnnotations;

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

        public DateTime? CreatedTime { get; set; }
	}
}
