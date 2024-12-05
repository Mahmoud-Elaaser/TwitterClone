using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.FollowDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IFollowService
    {
        Task<ResponseDto> FollowUserAsync(CreateFollowDto createFollowDto);
        Task<ResponseDto> UnfollowUserAsync(FollowDto followDto);
        Task<ResponseDto> UnfollowUserAsync(int followerId, int followingId);
        Task<ResponseDto> GetFollowersAsync(int userId);
        Task<ResponseDto> GetFollowingAsync(int userId);

    }
}
