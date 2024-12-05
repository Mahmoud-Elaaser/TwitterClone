using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs.FollowDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly IFollowService _followService;

        public FollowController(IFollowService followService)
        {
            _followService = followService;
        }

        [HttpPost("follow")]
        [SwaggerOperation(Summary = "Follow a user", Description = "Creates a follow relationship between two users.")]
        public async Task<IActionResult> FollowUser(CreateFollowDto createFollowDto)
        {
            var response = await _followService.FollowUserAsync(createFollowDto);
            if (!response.IsSucceeded)
                return BadRequest(new ApiResponse(response.Status, response.Message));

            return Ok(response.Message);
        }

        [HttpDelete("unfollow/{followerId}/{followingId}")]
        [SwaggerOperation(Summary = "Unfollow a user", Description = "Removes the follow relationship between two users.")]
        public async Task<IActionResult> UnfollowUser(int followerId, int followingId)
        {
            var response = await _followService.UnfollowUserAsync(followerId, followingId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response.Message);
        }



        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetFollowers(int userId)
        {
            var response = await _followService.GetFollowersAsync(userId);
            if (!response.IsSucceeded)
                return BadRequest(new ApiResponse(response.Status, response.Message));

            return Ok(response.Models);
        }


        [HttpGet("{userId}/followings")]
        public async Task<IActionResult> GetFollowings(int userId)
        {
            var response = await _followService.GetFollowingAsync(userId);
            if (!response.IsSucceeded)
                return BadRequest(new ApiResponse(response.Status, response.Message));

            return Ok(response.Models);
        }
    }
}
