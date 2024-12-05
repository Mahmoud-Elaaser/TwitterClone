using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        public AuthService(UserManager<User> userManager,
                           RoleManager<IdentityRole<int>> roleManager,
                           IMapper mapper,
                           IConfiguration configuration,
                           SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
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
                    Message = "something went wrong, during registeration"
                };
            }

            if (!string.IsNullOrEmpty(registerDto.RoleName))
            {
                /// if the role didn't exist => assign it to user
                if (!await _roleManager.RoleExistsAsync(registerDto.RoleName))
                    await _roleManager.CreateAsync(new IdentityRole<int> { Name = registerDto.RoleName });
                await _userManager.AddToRoleAsync(user, registerDto.RoleName);
            }

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Registrtion completed successfully",
                Model = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<ResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "something went wrong, during loging user"
                };
            }

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Logging completed successfully",
                Model = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<ResponseDto> AssignRoleAsync(RoleDto assignedRoleDto)
        {
            var user = await _userManager.FindByIdAsync(assignedRoleDto.UserId.ToString());
            if (user == null) throw new Exception("User not found.");

            if (!await _roleManager.RoleExistsAsync(assignedRoleDto.RoleName))
                await _roleManager.CreateAsync(new IdentityRole<int> { Name = assignedRoleDto.RoleName });
            var result = await _userManager.AddToRoleAsync(user, assignedRoleDto.RoleName);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "something went wrong, during adding role",
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
            if (user == null) throw new Exception("User not found.");

            var result = await _userManager.RemoveFromRoleAsync(user, removeRoleDto.RoleName);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
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
            if (user == null) throw new Exception("User not found.");

            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            var result = await AssignRoleAsync(updateRoleDto);
            if (!result.IsSucceeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
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
                    Message = "User not found",
                };
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "Something went wrong during updating password",
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
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiry = token.ValidTo
            };
        }


        /// Generate token for reset-password email
        public async Task<string> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new InvalidOperationException("User with the provided email does not exist.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<ResponseDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "Invalid user."
                };
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                };
            }

            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Password reset successful.",
                Model = result
            };
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

    }

}