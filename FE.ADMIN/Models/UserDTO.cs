using System.ComponentModel.DataAnnotations;

namespace FE.ADMIN.Models
{
    public class UserDTO
    {
        [Key]
        public string? ID { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
    }
}