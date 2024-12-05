using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Service.DTOs.AuthenticationDto
{
    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Bio { get; set; }

        public string UserName { get; set; }
        public string RoleName { get; set; }

        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare(nameof(Password), ErrorMessage = "Password Not Matching!")]
        [Display(Name = "Repeat Pawword ")]
        public string ConfirmPassword { get; set; }

    }
}
