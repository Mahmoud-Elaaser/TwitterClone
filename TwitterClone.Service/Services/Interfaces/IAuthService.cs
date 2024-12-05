using TwitterClone.Data.Entities;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.AuthenticationDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<ResponseDto> LoginAsync(LoginDto loginDto);
        Task<ResponseDto> AssignRoleAsync(RoleDto dto);
        Task<ResponseDto> RemoveRoleAsync(RoleDto dto);
        Task<ResponseDto> UpdateRoleAsync(RoleDto updateRoleDto);
        Task<ResponseDto> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
        Task<TokenDto> GenerateJwtTokenAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(string email);
        Task SignOutAsync();
    }
}
