using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MuteController : ControllerBase
    {
        private readonly IMuteUserService _muteUserService;

        public MuteController(IMuteUserService muteUserService)
        {
            _muteUserService = muteUserService;
        }

        [HttpPost("{mutedUserId}/mute")]
        public async Task<IActionResult> MuteUser(int mutedUserId)
        {
            var mutedById = GetCurrentUserId();
            var success = await _muteUserService.MuteUserAsync(mutedById, mutedUserId);

            if (!success)
                return BadRequest("User is already muted.");

            return Ok("User muted successfully.");
        }

        [HttpPost("{mutedUserId}/unmute")]
        public async Task<IActionResult> UnmuteUser(int mutedUserId)
        {
            var mutedById = GetCurrentUserId();
            var success = await _muteUserService.UnmuteUserAsync(mutedById, mutedUserId);

            if (!success)
                return BadRequest("User is not muted.");

            return Ok("User unmuted successfully.");
        }

        [HttpGet("{mutedUserId}/is-muted")]
        public async Task<IActionResult> IsUserMuted(int mutedUserId)
        {
            var mutedById = GetCurrentUserId();
            var isMuted = await _muteUserService.IsUserMutedAsync(mutedById, mutedUserId);

            return Ok(new { IsMuted = isMuted });
        }

        /// Get user Id from JWT or context
        private int GetCurrentUserId()
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            /// Check if the userIdClaim exists and can be parsed as an integer
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

            throw new InvalidOperationException("User Id claim is missing or invalid.");
        }

    }

}
