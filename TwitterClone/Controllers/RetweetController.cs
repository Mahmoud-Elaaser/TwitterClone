using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs.RetweetDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RetweetController : ControllerBase
    {
        private readonly IRetweetService _retweetService;

        public RetweetController(IRetweetService retweetService)
        {
            _retweetService = retweetService;
        }

        [HttpPost("retweet-tweet")]
        public async Task<IActionResult> AddRetweetAsync([FromBody] AddRetweetRequest request)
        {
            var response = await _retweetService.AddRetweetAsync(request.UserId, request.TweetId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Model);
        }

        [HttpGet("get-retweets/{tweetId}")]
        public async Task<ActionResult<IEnumerable<GetRetweetDto>>> GetRetweetsByTweetAsync(int tweetId)
        {
            var response = await _retweetService.GetRetweetsByTweetAsync(tweetId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Models);
        }

        [HttpGet("count-retweets/{tweetId}")]
        public async Task<IActionResult> CountRetweetsAsync(int tweetId)
        {
            var cntr = await _retweetService.CountRetweetsAsync(tweetId);

            return Ok($"Total number of retweet is: {cntr}");
        }

        [HttpGet("is-retweeted/{userId}/{tweetId}")]
        public async Task<IActionResult> IsRetweetedAsync(int userId, int tweetId)
        {
            var isRetweeted = await _retweetService.IsRetweetedAsync(userId, tweetId);

            return Ok(isRetweeted);
        }

        [HttpDelete("remove/{retweetId}")]
        public async Task<IActionResult> RemoveRetweetAsync(int retweetId)
        {
            var response = await _retweetService.RemoveRetweetAsync(retweetId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpGet("get-all-retweets-on-a-tweet")]
        public async Task<ActionResult<IEnumerable<GetRetweetDto>>> GetAllRetweetsOnTweet(int tweetId)
        {
            var response = await _retweetService.GetAllRetweetsOnTweetAsync(tweetId);
            return Ok(response.Models);
        }
    }
}
