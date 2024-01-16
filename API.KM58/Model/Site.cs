using System.ComponentModel.DataAnnotations;

namespace API.KM58.Model
{
    public class Site
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Project { get; set; }
        public int Point { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public int Round { get; set; }
        public string? Remarks { get; set; }
        public string? Ecremarks { get; set; }
        public string? CheckAccountAPI { get; set; }
        public string? PointClientAPI { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
