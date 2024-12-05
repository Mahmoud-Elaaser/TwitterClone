using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.QuoteDto;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface IQuoteService
    {
        Task<ResponseDto> AddQuoteAsync(CreateOrUpdateQuoteDto createQuoteDto);
        Task<ResponseDto> UpdateQuoteasync(int id, CreateOrUpdateQuoteDto updateQuoteDto);
        Task<ResponseDto> GetQuoteByIdAsync(int id);
        Task<ResponseDto> GetAllQuotesAsync();
        Task<ResponseDto> DeleteQuoteAsync(int id);
        Task<ResponseDto> GetAllQuotesOnTweetAsync(int tweetId);
    }
}
