using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.CommentDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface ICommentService
    {
        Task<ResponseDto> GetCommentByIdAsync(int id);
        Task<ResponseDto> GetAllCommentsAsync();
        Task<ResponseDto> AddCommentAsync(CreateOrUpdateCommentDto createCommentDto);
        Task<ResponseDto> UpdateCommentAsync(int id, CreateOrUpdateCommentDto updateCommentDto);
        Task<ResponseDto> DeleteCommentAsync(int id);
        Task<ResponseDto> GetAllCommentsOnTweetAsync(int tweetId);
    }
}
