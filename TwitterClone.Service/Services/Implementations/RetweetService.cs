using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.RetweetDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class RetweetService : IRetweetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RetweetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> AddRetweetAsync(int userId, int tweetId)
        {
            var existingRetweet = await _unitOfWork.Repository<Retweet>()
                .FindAsync(r => r.UserId == userId && r.TweetId == tweetId);

            if (existingRetweet != null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 400,
                    Message = "User has already retweeted this tweet."
                };
            }

            var retweet = new Retweet
            {
                UserId = userId,
                TweetId = tweetId
            };

            await _unitOfWork.Repository<Retweet>().AddAsync(retweet);
            await _unitOfWork.Complete();
            var mappedRetweet = _mapper.Map<GetRetweetDto>(retweet);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 201,
                Message = "You retweeted this tweet successfully.",
                Model = mappedRetweet
            };
        }

        public async Task<ResponseDto> GetRetweetsByTweetAsync(int tweetId)
        {
            var retweets = await _unitOfWork.Repository<Retweet>()
                .GetAllPredicated(r => r.TweetId == tweetId);

            var retweetDtos = _mapper.Map<IEnumerable<GetRetweetDto>>(retweets);

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Retweets retrieved successfully.",
                Models = retweetDtos
            };
        }

        public async Task<int> CountRetweetsAsync(int tweetId)
        {
            var retweetCount = await _unitOfWork.Repository<Retweet>()
                .CountAsync(r => r.TweetId == tweetId);

            return retweetCount;
        }

        /// Check if a tweet had been retweeted by a user
        public async Task<bool> IsRetweetedAsync(int userId, int tweetId)
        {
            var isRetweeted = await _unitOfWork.Repository<Retweet>()
                .AnyAsync(r => r.UserId == userId && r.TweetId == tweetId);

            return isRetweeted == true;
        }

        public async Task<ResponseDto> RemoveRetweetAsync(int retweetId)
        {
            var retweet = await _unitOfWork.Repository<Retweet>().GetByIdAsync(retweetId);
            if (retweet == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Retweet not found."
                };
            }

            _unitOfWork.Repository<Retweet>().Delete(retweet);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Retweet deleted successfully.",
            };
        }


        public async Task<ResponseDto> GetAllRetweetsOnTweetAsync(int tweetId)
        {
            var retweets = await _unitOfWork.Repository<Retweet>().GetAllPredicated(t => t.TweetId == tweetId);

            if (retweets == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Retweets not found"
                };
            }

            var mappedRetweets = _mapper.Map<IEnumerable<GetRetweetDto>>(retweets);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Retweets retrieved successfully",
                Models = mappedRetweets
            };
        }
    }
}
