using TwitterClone.Service.DTOs;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IRetweetService
    {
        Task<ResponseDto> AddRetweetAsync(int userId, int tweetId);
        Task<ResponseDto> GetRetweetsByTweetAsync(int tweetId);
        Task<int> CountRetweetsAsync(int tweetId);
        Task<bool> IsRetweetedAsync(int userId, int tweetId);
        Task<ResponseDto> RemoveRetweetAsync(int retweetId);
        Task<ResponseDto> GetAllRetweetsOnTweetAsync(int tweetId);
    }
}
