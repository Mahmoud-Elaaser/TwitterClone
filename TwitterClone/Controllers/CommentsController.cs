using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs.CommentDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<GetCommentDto>> Getallcomments()
        {
            var response = await _commentService.GetAllCommentsAsync();
            return Ok(response.Models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var response = await _commentService.GetCommentByIdAsync(id);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Model);
        }
        [HttpPost("add-comment")]
        public async Task<IActionResult> AddComment([FromBody] CreateOrUpdateCommentDto dto)
        {
            var response = await _commentService.AddCommentAsync(dto);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CreateOrUpdateCommentDto dto)
        {
            var response = await _commentService.UpdateCommentAsync(id, dto);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var response = await _commentService.DeleteCommentAsync(id);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpGet("get-all-comments-on-a-tweet")]
        public async Task<ActionResult<IEnumerable<GetCommentDto>>> GetAllCommentsOnTweet(int tweetId)
        {
            var response = await _commentService.GetAllCommentsOnTweetAsync(tweetId);
            return Ok(response.Models);
        }
    }
}
