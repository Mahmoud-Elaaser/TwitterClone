using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.AuthenticationDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
        Task<ResponseDto> VerifyEmail(ConfirmEmailDto confirmEmailDto);
        Task<ResponseDto> ResendEmailConfirmationTokenAsync(string Email);
    }
}
