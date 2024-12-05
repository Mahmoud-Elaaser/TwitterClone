using System.ComponentModel.DataAnnotations;

namespace TwitterClone.Service.DTOs.AuthenticationDto
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Please Enter Your Email!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter Your Email correctly!")]
        public string Email { get; set; }
    }
}
