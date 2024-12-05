using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.LikeDto;
using TwitterClone.Service.DTOs.TweetDto;
using TwitterClone.Service.DTOs.UserDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface ILikeService
    {
        Task<IEnumerable<GetUserDto>> GetUsersWhoLikedTweetAsync(int tweetId);
        Task<IEnumerable<GetTweetDto>> GetLikedTweetsAsync(int userId);
        Task<int> CountLikesAsync(int tweetId);
        Task<bool> IsLikedAsync(int tweetId, int userId);
        Task<ResponseDto> AddLikeAsync(CreateLikeDto addLikeDto);
        Task<ResponseDto> RemoveLikeAsync(int tweetId, int userId);
        Task<ResponseDto> GetAllLikesOnTweetAsync(int tweetId);
    }
}
