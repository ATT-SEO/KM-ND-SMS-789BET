using System.ComponentModel.DataAnnotations;

namespace FE.ADMIN.Models
{
    public class RegistrationRequestDTO
    {
        public string ID { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
