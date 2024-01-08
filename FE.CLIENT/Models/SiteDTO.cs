namespace FE.CLIENT.Models
{
    public class SiteDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool? Status { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
