using AutoMapper;
using System.Linq.Expressions;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.TweetDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class TweetService : ITweetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TweetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        #region Fetch Operations

        /// retrieve all statistcs of a specific twet such as: number of comments, likes, bookmarks, and retweets
        public async Task<ResponseDto<TweetStatisticsDto>> GetTweetStatisticsAsync(int tweetId)
        {
            var tweet = await _unitOfWork.Repository<Tweet>().GetByIdAsync(tweetId);
            if (tweet == null)
            {
                return new ResponseDto<TweetStatisticsDto>
                {
                    Success = false,
                    Message = "Tweet not found"
                };
            }

            /// Get counts
            var likesCount = await _unitOfWork.Repository<Like>().CountAsync(like => like.TweetId == tweetId);
            var commentsCount = await _unitOfWork.Repository<Comment>().CountAsync(comment => comment.TweetId == tweetId);
            var bookmarksCount = await _unitOfWork.Repository<Bookmark>().CountAsync(bookmark => bookmark.TweetId == tweetId);
            var retweetsCount = await _unitOfWork.Repository<Retweet>().CountAsync(retweet => retweet.TweetId == tweetId);

            var statisticsDto = new TweetStatisticsDto
            {
                TweetId = tweetId,
                Likes = likesCount,
                Comments = commentsCount,
                Bookmarks = bookmarksCount,
                Retweets = retweetsCount
            };

            return new ResponseDto<TweetStatisticsDto>
            {
                Success = true,
                Message = "Statistics retrieved successfully",
                Data = statisticsDto
            };
        }

        public async Task<ResponseDto> GetAllTweetsAsync()
        {
            var tweets = await _unitOfWork.Repository<Tweet>().GetAllAsync();
            var mappedTweets = _mapper.Map<IEnumerable<GetTweetDto>>(tweets);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = mappedTweets
            };
        }
        public async Task<ResponseDto> GetUserFeedAsync(int userId)
        {
            /// Fetch all muted users and filter in memory
            var allMutedUsers = await _unitOfWork.Repository<MuteUser>().GetAllAsync();
            var mutedUserIds = allMutedUsers.Where(mu => mu.MutedById == userId).Select(mu => mu.MutedUserId).ToList();

            /// Fetch all tweets and filter [All tweets except MutedUser's tweet]
            var allTweets = await _unitOfWork.Repository<Tweet>().GetAllAsync();
            var filteredTweets = allTweets.Where(t => !mutedUserIds.Contains(t.UserId));

            var mappedTweets = _mapper.Map<IEnumerable<GetTweetDto>>(filteredTweets);

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = mappedTweets
            };
        }


        public async Task<ResponseDto> GetTweetByIdAsync(int id)
        {
            var tweet = await _unitOfWork.Repository<Tweet>().GetByIdAsync(id);
            if (tweet == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Tweet not found"
                };
            }

            var mappedTweet = _mapper.Map<GetTweetDto>(tweet);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Model = mappedTweet
            };
        }

        public async Task<ResponseDto> GetTweetsWithSpecAsync(ISpecifications<Tweet> spec)
        {
            var tweets = await _unitOfWork.Repository<Tweet>().GetAllAsyncWithSpec(spec);
            var mappedTweets = _mapper.Map<IEnumerable<GetTweetDto>>(tweets);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = mappedTweets
            };
        }

        public async Task<ResponseDto> FindTweetAsync(Expression<Func<Tweet, bool>> predicate)
        {
            var tweet = await _unitOfWork.Repository<Tweet>().FindAsync(predicate);
            if (tweet == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "No predicated tweets found"
                };
            }

            var mappedTweet = _mapper.Map<GetTweetDto>(tweet);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Model = mappedTweet
            };
        }

        public async Task<ResponseDto> GetTweetsByPredicateAsync(Expression<Func<Tweet, bool>> predicate, string[] includes = null)
        {
            var tweets = await _unitOfWork.Repository<Tweet>().GetAllPredicated(predicate, includes);
            var mappedTweets = _mapper.Map<IEnumerable<GetTweetDto>>(tweets);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = mappedTweets
            };
        }

        #endregion

        #region Add Operations

        public async Task<ResponseDto> AddTweetAsync(CreateOrUpdateTweetDto createTweetDto)
        {
            var tweet = _mapper.Map<Tweet>(createTweetDto);
            await _unitOfWork.Repository<Tweet>().AddAsync(tweet);
            await _unitOfWork.Complete();

            var mappedTweet = _mapper.Map<CreateOrUpdateTweetDto>(tweet);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Model = mappedTweet
            };
        }

        public async Task<ResponseDto> AddMultipleTweetsAsync(IEnumerable<CreateOrUpdateTweetDto> createTweetDtos)
        {
            var tweets = _mapper.Map<ICollection<Tweet>>(createTweetDtos);
            await _unitOfWork.Repository<Tweet>().AddRangeAsync(tweets);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Tweets created successfully"
            };
        }

        #endregion

        #region Update Operations

        public async Task<ResponseDto> UpdateTweetAsync(int id, CreateOrUpdateTweetDto updateTweetDto)
        {
            var existingTweet = await _unitOfWork.Repository<Tweet>().GetByIdAsync(id);
            if (existingTweet == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Tweet not found"
                };
            }

            _mapper.Map(updateTweetDto, existingTweet);
            _unitOfWork.Repository<Tweet>().Update(existingTweet);
            await _unitOfWork.Complete();

            var mappedTweet = _mapper.Map<GetTweetDto>(existingTweet);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Tweet updated successfully"
            };
        }

        public async Task<ResponseDto> UpdateMultipleTweetsAsync(IEnumerable<CreateOrUpdateTweetDto> updateTweetDtos)
        {
            var tweets = _mapper.Map<ICollection<Tweet>>(updateTweetDtos);
            await _unitOfWork.Repository<Tweet>().UpdateRangeAsync(tweets);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Tweets updated successfully."
            };
        }

        #endregion

        #region Delete Operations

        public async Task<ResponseDto> DeleteTweetAsync(int id)
        {
            var tweet = await _unitOfWork.Repository<Tweet>().GetByIdAsync(id);
            if (tweet == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Tweet not found"
                };
            }

            _unitOfWork.Repository<Tweet>().Delete(tweet);
            await _unitOfWork.Complete();
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Tweet deleted successfully."
            };
        }

        public async Task<ResponseDto> DeleteMultipleTweetsAsync(IEnumerable<int> ids)
        {
            var tweets = ids.Select(id => _unitOfWork.Repository<Tweet>().GetByIdAsync(id).Result).Where(t => t != null).ToList();
            if (!tweets.Any())
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Tweets not found"
                };
            }

            await _unitOfWork.Repository<Tweet>().DeleteRangeAsync(tweets);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Tweets deleted successfully"
            };
        }

        #endregion

    }
}

