using System.ComponentModel.DataAnnotations;

namespace FE.ADMIN.Models
{
    public class LoginRequestDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string ProjectCode { get; set; }
    }
}
