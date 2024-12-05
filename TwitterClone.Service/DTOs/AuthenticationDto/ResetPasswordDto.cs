using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Service.DTOs.AuthenticationDto
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }

}
