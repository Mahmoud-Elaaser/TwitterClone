using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs.QuoteDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteService _quoteService;

        public QuotesController(IQuoteService quoteService)
        {
            _quoteService = quoteService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<GetQuoteDto>>> GetAllQuotes()
        {
            var response = await _quoteService.GetAllQuotesAsync();
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetQuoteById(int id)
        {
            var response = await _quoteService.GetQuoteByIdAsync(id);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response.Model);
        }

        [HttpPost("add-quote")]
        public async Task<IActionResult> AddQuote([FromBody] CreateOrUpdateQuoteDto createQuoteDto)
        {
            var response = await _quoteService.AddQuoteAsync(createQuoteDto);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuote(int id, CreateOrUpdateQuoteDto updateQuoteDto)
        {
            var response = await _quoteService.UpdateQuoteasync(id, updateQuoteDto);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var response = await _quoteService.DeleteQuoteAsync(id);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response.Message);
        }


        [HttpGet("get-all-quotes-on-a-tweet")]
        public async Task<ActionResult<IEnumerable<GetQuoteDto>>> GetAllQuotesOnTweet(int tweetId)
        {
            var response = await _quoteService.GetAllQuotesOnTweetAsync(tweetId);
            return Ok(response.Models);
        }
    }
}
