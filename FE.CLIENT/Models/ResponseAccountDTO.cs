namespace FE.CLIENT.Models
{
    public class ResponseAccountDTO
    {
        public int Status_Code { get; set; }
        public bool Valid { get; set; }
        public string? PlayerId { get; set; }
        public string? FirstName { get; set; }
        public string? Mobile { get; set; }
        public int totaldeposit { get; set; }
        public int totalwithdraw { get; set; }
        public int totaldepositcount { get; set; }
        public int totalwithdrawcount { get; set; }
        public long CreateDate { get; set; }

    }
}
