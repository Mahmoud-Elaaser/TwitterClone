using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.LikeDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LikeController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikeController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("add-like")]
        public async Task<IActionResult> AddLike(CreateLikeDto dto)
        {
            var result = await _likeService.AddLikeAsync(dto);
            if (!result.IsSucceeded)
            {
                return BadRequest(
                    new ResponseDto
                    {
                        Message = "Could not add like. The user might be blocked or the like already exists.",
                        IsSucceeded = false,
                        Status = result.Status
                    }
                );
            }

            return Ok(result.Message);
        }

        [HttpDelete("remove-like")]
        public async Task<IActionResult> RemoveLike(int tweetId, int userId)
        {
            var result = await _likeService.RemoveLikeAsync(tweetId, userId);
            if (!result.IsSucceeded)
            {
                return BadRequest(
                    new ResponseDto
                    {
                        Message = "Could not remove like. It may not exist.",
                        IsSucceeded = false,
                        Status = result.Status
                    }
                );
            }

            return Ok(result.Message);
        }

        [HttpGet("count-likes")]
        public async Task<IActionResult> CountLikes(int tweetId)
        {
            var count = await _likeService.CountLikesAsync(tweetId);
            return Ok(new { TweetId = tweetId, LikesCount = count });
        }

        [HttpGet("isLiked")]
        public async Task<IActionResult> IsLiked(int tweetId, int userId)
        {
            var isLiked = await _likeService.IsLikedAsync(tweetId, userId);
            return Ok(new { TweetId = tweetId, IsLiked = isLiked });
        }

        [HttpGet("get-who-liked-a-tweet")]
        public async Task<IActionResult> GetUsersWhoLikedTweet(int tweetId)
        {
            var users = await _likeService.GetUsersWhoLikedTweetAsync(tweetId);
            if (users == null || !users.Any())
            {
                return BadRequest(
                    new ResponseDto
                    {
                        Message = "No users liked this tweet",
                        IsSucceeded = false,
                        Status = 404
                    }
                );
            }

            return Ok(users);
        }

        [HttpGet("liked-tweets-by-user")]
        public async Task<IActionResult> GetLikedTweets(int userId)
        {
            var likedTweets = await _likeService.GetLikedTweetsAsync(userId);
            if (likedTweets == null || !likedTweets.Any())
            {
                return BadRequest(
                    new ResponseDto
                    {
                        Message = "No liked tweets found.",
                        IsSucceeded = false,
                        Status = 404
                    }
                );
            }

            return Ok(likedTweets);
        }

        [HttpGet("all-likes-on-a-tweet")]
        public async Task<ActionResult<IEnumerable<GetLikeDto>>> GetAllLikesOnTweet(int tweetId)
        {
            var response = await _likeService.GetAllLikesOnTweetAsync(tweetId);
            return Ok(response.Models);
        }

    }
}