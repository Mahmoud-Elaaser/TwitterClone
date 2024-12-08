using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs.BookmarkDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarksController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpPost("add-bookmark")]
        public async Task<IActionResult> AddBookmark([FromBody] CreateBookmarkDto createBookmarkDto)
        {
            var response = await _bookmarkService.AddBookmarkAsync(createBookmarkDto);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBookmark(int id)
        {
            var response = await _bookmarkService.RemoveBookmarkAsync(id);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetBookmarkDto>>> GetUserBookmarks(int userId)
        {
            var response = await _bookmarkService.GetUserBookmarksAsync(userId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Models);
        }

        [HttpGet("tweet/{tweetId}/count")]
        public async Task<IActionResult> CountBookmarksOfTweet(int tweetId)
        {
            int count = await _bookmarkService.CountBookmarksOfTweetAsync(tweetId);
            return Ok(count);
        }

        [HttpGet("user/{userId}/tweet/{tweetId}/isBookmarked")]
        public async Task<IActionResult> IsBookmarked(int userId, int tweetId)
        {
            bool isBookmarked = await _bookmarkService.IsBookmarkedAsync(userId, tweetId);
            return Ok(isBookmarked);
        }
    }
}
