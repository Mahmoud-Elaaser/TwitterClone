using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlockController : ControllerBase
    {
        private readonly IBlockService _blockService;

        public BlockController(IBlockService blockService)
        {
            _blockService = blockService;
        }
        [HttpPost("block")]
        public async Task<IActionResult> BlockUser([FromBody] BlockUserDto blockUserDto)
        {
            var response = await _blockService.BlockUserAsync(blockUserDto.BlockedById, blockUserDto.BlockedUserId);
            return Ok(response.Message);
        }

        [HttpPost("unblock")]
        public async Task<IActionResult> UnblockUser([FromBody] BlockUserDto blockUserDto)
        {
            var response = await _blockService.UnblockUserAsync(blockUserDto.BlockedById, blockUserDto.BlockedUserId);
            return Ok(response.Message);
        }
    }
}
