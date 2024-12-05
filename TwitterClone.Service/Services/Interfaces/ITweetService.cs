using System.Linq.Expressions;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.TweetDto;


namespace TwitterClone.Service.Services.Interfaces
{
    public interface ITweetService
    {
        Task<ResponseDto> GetAllTweetsAsync();
        Task<ResponseDto> GetTweetByIdAsync(int id);
        Task<ResponseDto> GetTweetsWithSpecAsync(ISpecifications<Tweet> spec);
        Task<ResponseDto> FindTweetAsync(Expression<Func<Tweet, bool>> predicate);
        Task<ResponseDto> GetTweetsByPredicateAsync(Expression<Func<Tweet, bool>> predicate, string[] includes = null);
        Task<ResponseDto> AddTweetAsync(CreateOrUpdateTweetDto dto);
        Task<ResponseDto> AddMultipleTweetsAsync(IEnumerable<CreateOrUpdateTweetDto> dto);
        Task<ResponseDto> UpdateTweetAsync(int id, CreateOrUpdateTweetDto dto);
        Task<ResponseDto> UpdateMultipleTweetsAsync(IEnumerable<CreateOrUpdateTweetDto> dto);
        Task<ResponseDto> DeleteTweetAsync(int id);
        Task<ResponseDto> DeleteMultipleTweetsAsync(IEnumerable<int> ids);
        Task<ResponseDto> GetUserFeedAsync(int userId);
        Task<ResponseDto<TweetStatisticsDto>> GetTweetStatisticsAsync(int tweetId);

    }
}
