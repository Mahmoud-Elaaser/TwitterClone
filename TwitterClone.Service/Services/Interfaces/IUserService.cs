using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.UserDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDto> CreateUserAsync(CreateOrUpdateUserDto dto);
        Task<ResponseDto> GetUserByIdAsync(int userId);
        Task<ResponseDto> GetAllUsersAsync();
        Task<ResponseDto> UpdateUserAsync(int userId, CreateOrUpdateUserDto dto);
        Task<ResponseDto> DeleteUserAsync(int userId);

    }
}
