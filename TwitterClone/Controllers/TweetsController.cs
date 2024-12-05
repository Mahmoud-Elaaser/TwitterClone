using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.TweetDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetService _tweetService;

        public TweetsController(ITweetService tweetService)
        {
            _tweetService = tweetService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetTweetDto>>> GetAllTweets()
        {
            var response = await _tweetService.GetAllTweetsAsync();
            return Ok(response.Models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTweetById(int id)
        {
            var response = await _tweetService.GetTweetByIdAsync(id);
            return Ok(response.Model);
        }


        /// Retrieve all tweets except tweets from user you muted
        [HttpGet("feed")]
        public async Task<IActionResult> GetFeed()
        {
            /// get user from claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var feed = await _tweetService.GetUserFeedAsync(int.Parse(userId));

            return Ok(feed.Models);
        }


        [HttpGet("search")]
        public async Task<IActionResult> GetTweetsByContent([FromQuery] string content)
        {
            var response = await _tweetService.GetTweetsByPredicateAsync(t => t.Content.Contains(content));
            return Ok(response.Models);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTweet([FromBody] CreateOrUpdateTweetDto createTweetDto)
        {
            var response = await _tweetService.AddTweetAsync(createTweetDto);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTweet(int id, [FromBody] CreateOrUpdateTweetDto updateTweetDto)
        {
            var response = await _tweetService.UpdateTweetAsync(id, updateTweetDto);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTweet(int id)
        {
            var response = await _tweetService.DeleteTweetAsync(id);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpGet("{tweetId}/statistics")]
        public async Task<ActionResult<IEnumerable<ResponseDto>>> GetTweetStatistics(int tweetId)
        {
            var response = await _tweetService.GetTweetStatisticsAsync(tweetId);

            if (!response.Success)
            {
                return BadRequest(new ApiResponse(404, response.Message));
            }

            return Ok(response.Data);
        }

    }
}
