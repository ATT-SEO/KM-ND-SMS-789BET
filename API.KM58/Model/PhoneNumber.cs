using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.KM58.Model
{
    public class PhoneNumber
    {
        [Key]
        public int Id { get; set; }
        public string? Number { get; set; }

        public string? Device { get; set; }
        public bool? Status { get; set; }

        public int SiteID { get; set; }
        [ForeignKey("SiteID")]
        public Site? Site { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
