using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KM58.Model
{
	public class Agent
	{
		[Key]
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Link { get; set; }
		public bool? Status { get; set; }
		public int MinPoint { get; set; }
		public int MaxPoint { get; set; }
		public int SiteID { get; set; }
		[ForeignKey("SiteID")]
		public Site? Site { get; set; }
		public string? Admin { get; set; }
		public DateTime? CreatedTime { get; set; }
		public DateTime? UpdatedTime { get; set; }
	}
}
