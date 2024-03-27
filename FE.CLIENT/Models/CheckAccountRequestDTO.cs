namespace FE.CLIENT.Models
{
    public class CheckAccountRequestDTO
    {
        public string? Account { get; set; }
        public string? Regfingerprint { get; set; }
        public string? IP { get; set; }
        public string? FP { get; set; }
        public string? Project { get; set; }
        public string? Token { get; set; }
        public string? RecaptchaToken { get; set; }

    }
}
