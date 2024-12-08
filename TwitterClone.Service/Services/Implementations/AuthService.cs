﻿using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.AuthenticationDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private static ConcurrentDictionary<string, string> OtpStorage = new ConcurrentDictionary<string, string>();

        public AuthService(UserManager<User> userManager,
                           RoleManager<IdentityRole<int>> roleManager,
                           IMapper mapper,
                           IConfiguration configuration,
                           SignInManager<User> signInManager,
                           IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<ResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "Something went wrong, during registeration process"
                };
            }

            if (!string.IsNullOrEmpty(registerDto.RoleName))
            {
                var isExist = await _roleManager.RoleExistsAsync(registerDto.RoleName);
                if (!isExist)
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = registerDto.RoleName });

                /// if the role didn't exist => assign it to user
                await _userManager.AddToRoleAsync(user, registerDto.RoleName);
            }

            var token = await GenerateJwtTokenAsync(user);

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Registrtion completed successfully",
                Model = token
            };
        }

        public async Task<ResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            var isMatching = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (user == null || isMatching == false)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "Invalid email or password"
                };
            }

            var token = await GenerateJwtTokenAsync(user);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Logging completed successfully",
                Model = token
            };
        }

        public async Task<ResponseDto> AssignRoleAsync(RoleDto assignedRoleDto)
        {
            var user = await _userManager.FindByIdAsync(assignedRoleDto.UserId.ToString());
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "User not found"
                };
            }

            var isExist = await _roleManager.RoleExistsAsync(assignedRoleDto.RoleName);
            if (!isExist)
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = assignedRoleDto.RoleName });

            var result = await _userManager.AddToRoleAsync(user, assignedRoleDto.RoleName);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 400,
                    Message = "something went wrong, during assigning role",
                };
            }
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Role assigned successfully",
            };
        }

        public async Task<ResponseDto> RemoveRoleAsync(RoleDto removeRoleDto)
        {
            var user = await _userManager.FindByIdAsync(removeRoleDto.UserId.ToString());
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Invalid Id, or this id not found"
                };
            };

            var result = await _userManager.RemoveFromRoleAsync(user, removeRoleDto.RoleName);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 400,
                    Message = "something went wrong, during romoving role",
                };
            }
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Role removed successfully",
            };
        }

        public async Task<ResponseDto> UpdateRoleAsync(RoleDto updateRoleDto)
        {
            var user = await _userManager.FindByIdAsync(updateRoleDto.UserId.ToString());
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Invalid Id, or this id not found"
                };
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var result = await AssignRoleAsync(updateRoleDto);
            if (!result.IsSucceeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 400,
                    Message = "something went wrong, during updating role",
                };
            }
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Role updated successfully",
            };
        }

        public async Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDto.UserId.ToString());
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "User not found",
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 400,
                    Message = "Something went wrong during changing password",
                };
            }

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Password changed successfully",
            };
        }

        public async Task<TokenDto> GenerateJwtTokenAsync(User user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(3), // Change it after finishing development stage
                signingCredentials: creds);

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = token.ValidTo
            };
        }


        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }



        // This function for forgot-password it takes an email and sends otp code for this email
        public async Task<ResponseDto> ForegotPassword(ForgotPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 400,
                    Message = "Invalid email"
                };
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var otpCode = GenerateOtpCode(6);
            var toEmail = dto.Email;
            var subject = "Forgot Password Request";
            var body = $"Your OTP Code is:\a{otpCode}\a Don't share it with anyone";

            await _emailService.SendEmailAsync(toEmail, subject, body);
            OtpStorage[dto.Email] = otpCode;

            return new ResponseDto
            {
                Status = 200,
                IsSucceeded = true,
                Message = "We sent you an email, please check it"
            };
        }

        // This function for reset-password it takes otp code that have been sent to email then you can reset your password
        public async Task<ResponseDto> RessetPassword(ResetPasswordDto dto)
        {
            var response = new ResponseDto();
            if (!OtpStorage.TryGetValue(dto.Email, out var otpstorage) || otpstorage != dto.Code)
            {
                response.Status = 400;
                response.Message = "Invalid OtpCode please try again!";
                return response;
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                var hashPassword = _userManager.PasswordHasher.HashPassword(user, dto.NewPassword);
                user.PasswordHash = hashPassword;
                await _userManager.ChangePasswordAsync(user, user.PasswordHash, dto.NewPassword);
                await _userManager.UpdateAsync(user);
                OtpStorage.TryRemove(dto.Email, out _);
                response.IsSucceeded = true;
                response.Status = 200;
                response.Message = "Password has been changed successfully";
                return response;
            }
            response.Status = 400;
            response.Message = "Invalid User!";
            return response;
        }

        private string GenerateOtpCode(int lenth)
        {
            var random = new Random();
            string Otp = string.Empty;
            for (int r = 0; r < lenth; r++)
            {
                Otp += random.Next(0, 10).ToString();

            }
            return Otp;
        }
    }

}