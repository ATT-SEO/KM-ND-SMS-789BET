namespace FE.JunCMD.Client.Models
{
    public class ResponseAccountDTO
    {
        public int Status_Code { get; set; }
        public bool Valid { get; set; }
        public string? Login { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
