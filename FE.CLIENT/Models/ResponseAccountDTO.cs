namespace FE.CLIENT.Models
{
    public class ResponseAccountDTO
    {
        public int Status_Code { get; set; }
        public bool Valid { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
    }
}
