using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.QuoteDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class QuoteService : IQuoteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuoteService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddQuoteAsync(CreateOrUpdateQuoteDto createQuoteDto)
        {
            var quote = _mapper.Map<Quote>(createQuoteDto);
            quote.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Quote>().AddAsync(quote);
            await _unitOfWork.Complete();

            var mappedQuote = _mapper.Map<GetQuoteDto>(quote);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Model = mappedQuote,
                Message = "Quote created successfully."
            };
        }

        public async Task<ResponseDto> GetQuoteByIdAsync(int id)
        {
            var quote = await _unitOfWork.Repository<Quote>().GetByIdAsync(id);
            if (quote == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Quote not found."
                };
            }

            var mappedQuote = _mapper.Map<GetQuoteDto>(quote);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Model = mappedQuote
            };
        }

        public async Task<ResponseDto> GetAllQuotesAsync()
        {
            var quotes = await _unitOfWork.Repository<Quote>().GetAllAsync();
            var mappedQuotes = _mapper.Map<IEnumerable<GetQuoteDto>>(quotes);

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = mappedQuotes
            };
        }

        public async Task<ResponseDto> UpdateQuoteasync(int id, CreateOrUpdateQuoteDto updateQuoteDto)
        {
            var existingQuote = await _unitOfWork.Repository<Quote>().GetByIdAsync(id);
            if (existingQuote == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Quote not found"
                };
            }

            _mapper.Map(updateQuoteDto, existingQuote);
            _unitOfWork.Repository<Quote>().Update(existingQuote);
            await _unitOfWork.Complete();

            var mappedTweet = _mapper.Map<GetQuoteDto>(existingQuote);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Quote updated successfully"
            };
        }

        public async Task<ResponseDto> DeleteQuoteAsync(int id)
        {
            var quote = await _unitOfWork.Repository<Quote>().GetByIdAsync(id);
            if (quote == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Quote not found."
                };
            }

            _unitOfWork.Repository<Quote>().Delete(quote);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Quote deleted successfully."
            };
        }

        public async Task<ResponseDto> GetAllQuotesOnTweetAsync(int tweetId)
        {
            var quotes = await _unitOfWork.Repository<Quote>().GetAllPredicated(t => t.TweetId == tweetId);

            if (quotes == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "quotes not found"
                };
            }

            var mappedQuotes = _mapper.Map<IEnumerable<GetQuoteDto>>(quotes);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Quotes retrieved successfully",
                Models = mappedQuotes
            };
        }
    }
}
