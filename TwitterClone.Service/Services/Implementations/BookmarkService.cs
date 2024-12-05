using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.BookmarkDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookmarkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddBookmarkAsync(CreateBookmarkDto createBookmarkDto)
        {
            var bookmark = _mapper.Map<Bookmark>(createBookmarkDto);
            await _unitOfWork.Repository<Bookmark>().AddAsync(bookmark);
            await _unitOfWork.Complete();

            var mappedBookmark = _mapper.Map<GetBookmarkDto>(bookmark);
            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "You bookmarked this tweet successfully.",
                Model = mappedBookmark
            };
        }

        public async Task<ResponseDto> RemoveBookmarkAsync(int bookmarkId)
        {
            var bookmark = await _unitOfWork.Repository<Bookmark>().GetByIdAsync(bookmarkId);
            if (bookmark == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Bookmark not found."
                };
            }

            _unitOfWork.Repository<Bookmark>().Delete(bookmark);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Bookmark removed successfully."
            };
        }

        /// retrieve all user's bookmarks
        public async Task<ResponseDto> GetUserBookmarksAsync(int userId)
        {
            var bookmarks = await _unitOfWork.Repository<Bookmark>()
                .GetAllPredicated(b => b.UserId == userId);
            if (bookmarks == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "No bookmarks found for this user."
                };
            }

            var bookmarkDtos = _mapper.Map<IEnumerable<GetBookmarkDto>>(bookmarks);

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = bookmarkDtos
            };
        }

        /// retrieve the total number of bookmarks on a tweet
        public async Task<int> CountBookmarksOfTweetAsync(int tweetId)
        {
            var count = await _unitOfWork.Repository<Bookmark>()
                .CountAsync(b => b.TweetId == tweetId);

            return count;
        }

        /// check if the user bookmarked a specific tweet or not
        public async Task<bool> IsBookmarkedAsync(int userId, int tweetId)
        {
            var isBookmarked = await _unitOfWork.Repository<Bookmark>()
                .AnyAsync(b => b.UserId == userId && b.TweetId == tweetId);

            return isBookmarked == true;
        }
    }

}