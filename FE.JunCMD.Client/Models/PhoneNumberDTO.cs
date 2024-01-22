namespace FE.JunCMD.Client.Models
{
    public class PhoneNumberDTO
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string? Device { get; set; }
        public bool? Status { get; set; }
        public int SiteID { get; set; }
        public DateTime? CreatedTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
    }
}
