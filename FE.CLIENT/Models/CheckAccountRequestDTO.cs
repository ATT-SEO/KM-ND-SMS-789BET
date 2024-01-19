namespace FE.CLIENT.Models
{
    public class CheckAccountRequestDTO
    {
        public string? Account { get; set; }
        public string? Regfingerprint { get; set; }
        public string? RecaptchaToken { get; set; }

    }
}
