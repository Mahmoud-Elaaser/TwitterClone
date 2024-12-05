using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Service.DTOs.AuthenticationDto
{
    public class TokenRequestModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
