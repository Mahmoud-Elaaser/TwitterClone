using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.BookmarkDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IBookmarkService
    {
        Task<ResponseDto> AddBookmarkAsync(CreateBookmarkDto createBookmarkDto);
        Task<ResponseDto> RemoveBookmarkAsync(int bookmarkId);
        Task<ResponseDto> GetUserBookmarksAsync(int userId);
        Task<int> CountBookmarksOfTweetAsync(int tweetId);
        Task<bool> IsBookmarkedAsync(int userId, int tweetId);
    }

}
