using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model
{
    public class Site
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Project { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
