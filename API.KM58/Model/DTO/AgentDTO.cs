using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model.DTO
{
    public class AgentDTO
    {
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Link { get; set; }
		public bool? Status { get; set; }
		public int MinPoint { get; set; }
		public int MaxPoint { get; set; }
		public int SiteID { get; set; }
		public Site? Site { get; set; }
		public string? Admin { get; set; }
		public DateTime? CreatedTime { get; set; }
		public DateTime? UpdatedTime { get; set; }
	}
}
