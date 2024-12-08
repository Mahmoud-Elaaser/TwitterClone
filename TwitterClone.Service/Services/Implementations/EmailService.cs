using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.AuthenticationDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<DataProtectionTokenProviderOptions> _tokenProviderOptions;
        private readonly IUserService _userService;

        public EmailService(IConfiguration config, UserManager<User> userManager,
            IOptions<DataProtectionTokenProviderOptions> tokenProviderOptions,
            IUserService userService)
        {
            _config = config;
            _userManager = userManager;
            _tokenProviderOptions = tokenProviderOptions;
            _userService = userService;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailSettings:Email"]));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]!),
                MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailSettings:Email"], _config["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task<ResponseDto> VerifyEmail(ConfirmEmailDto confirmEmailDto)
        {

            if (string.IsNullOrEmpty(confirmEmailDto.Email) || string.IsNullOrEmpty(confirmEmailDto.Token))
                return new ResponseDto { IsSucceeded = false, Message = "User ID and token are required." };

            var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);
            //if (user == null || user.IsDeleted)
            //    return new ResponseDto { IsSucceeded = false, Message = "User not found." };
            if (user.EmailConfirmed)
                return new ResponseDto { Message = "Your Email is already confirmed" };
            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);
            if (!result.Succeeded)
                return new ResponseDto { IsSucceeded = false, Message = "Token is not valid!" };

            return new ResponseDto { IsSucceeded = true, Message = "Your Email has been confirmed successfully :) " };
        }

        public async Task<ResponseDto> ResendEmailConfirmationTokenAsync(string Email)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            //if (user == null || user.IsDeleted)
            //    return new ResponseDto { IsSucceeded = false, Message = "User not found." };

            if (await _userManager.IsEmailConfirmedAsync(user))
                return new ResponseDto { IsSucceeded = true, Message = "Email is already confirmed." };

            // Generate new token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var expirationTime = _tokenProviderOptions.Value.TokenLifespan.TotalMinutes;
            // Send the new token via email
            await SendEmailAsync(user.Email, "Email Verification Code",
                $"Hello {user.UserName}, Use this new token to verify your Email: {token}\n This code is Valid only for {expirationTime} Minutes.");

            return new ResponseDto { IsSucceeded = false, Message = "A new verification email has been sent." };
        }




    }
}
