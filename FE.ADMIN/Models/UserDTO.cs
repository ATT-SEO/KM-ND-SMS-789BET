using System.ComponentModel.DataAnnotations;

namespace FE.ADMIN.Models
{
    public class UserDTO
    {
        public String UserName { get; set; }
        public String Role { get; set; }
        public String ProjectCode { get; set; }
    }
}