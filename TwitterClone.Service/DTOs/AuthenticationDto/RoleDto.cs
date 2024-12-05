using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Service.DTOs.AuthenticationDto
{
    public class RoleDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string RoleName { get; set; }
    }

}
