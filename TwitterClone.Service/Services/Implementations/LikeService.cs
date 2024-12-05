using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.LikeDto;
using TwitterClone.Service.DTOs.TweetDto;
using TwitterClone.Service.DTOs.UserDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class LikeService : ILikeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public LikeService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        /// Get a list of users who liked a specific tweet
        public async Task<IEnumerable<GetUserDto>> GetUsersWhoLikedTweetAsync(int tweetId)
        {
            var likes = await _unitOfWork.Repository<Like>().GetAllPredicated(l => l.TweetId == tweetId);
            var userIds = likes.Select(l => l.UserId).Distinct();
            var users = await _unitOfWork.Repository<User>().GetAllPredicated(u => userIds.Contains(u.Id));
            return _mapper.Map<IEnumerable<GetUserDto>>(users);
        }

        /// Get tweets liked by a specific user
        public async Task<IEnumerable<GetTweetDto>> GetLikedTweetsAsync(int userId)
        {
            var likes = await _unitOfWork.Repository<Like>().GetAllPredicated(l => l.UserId == userId);
            var tweetIds = likes.Select(l => l.TweetId).Distinct();
            var tweets = await _unitOfWork.Repository<Tweet>().GetAllPredicated(t => tweetIds.Contains(t.Id));
            return _mapper.Map<IEnumerable<GetTweetDto>>(tweets);
        }

        public async Task<int> CountLikesAsync(int tweetId)
        {
            return await _unitOfWork.Repository<Like>().CountAsync(l => l.TweetId == tweetId);
        }

        /// Check if a user has liked a specific tweet
        public async Task<bool> IsLikedAsync(int tweetId, int userId)
        {
            var like = await _unitOfWork.Repository<Like>().FindAsync(l =>
                l.TweetId == tweetId && l.UserId == userId);
            return like != null;
        }

        public async Task<ResponseDto> AddLikeAsync(CreateLikeDto addLikeDto)
        {
            /// check if the author of the tweet blocked you 
            var isBlocked = await _unitOfWork.Repository<BlockUser>().AnyAsync(b =>
                b.BlockedUserId == addLikeDto.UserId && b.BlockedById == addLikeDto.UserId);
            if (isBlocked)
            {
                return new ResponseDto
                {
                    Message = "You are blocked by the author of this tweet.",
                    IsSucceeded = false,
                    Status = 401
                };
            }

            var existingLike = await _unitOfWork.Repository<Like>().FindAsync(l =>
                l.TweetId == addLikeDto.TweetId && l.UserId == addLikeDto.UserId);

            /// check if tweet is already liked
            if (existingLike != null)
            {
                return new ResponseDto
                {
                    Message = "You already liked this tweet.",
                    IsSucceeded = false,
                    Status = 401
                };
            }

            var like = _mapper.Map<Like>(addLikeDto);
            like.LikedAt = DateTime.UtcNow;

            await _unitOfWork.Repository<Like>().AddAsync(like);
            await _unitOfWork.Complete();

            /// Notification: User with id liked your tweet
            await _notificationService.SendLikeNotificationAsync(like.UserId, like.TweetId);

            var mappedLike = _mapper.Map<CreateLikeDto>(like);
            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Like created successfully",
                Model = mappedLike
            };
        }

        public async Task<ResponseDto> RemoveLikeAsync(int tweetId, int userId)
        {
            var like = await _unitOfWork.Repository<Like>().FindAsync(l =>
                l.TweetId == tweetId && l.UserId == userId);

            if (like == null)
            {
                return new ResponseDto
                {
                    Message = "Tweet not found",
                    IsSucceeded = false,
                    Status = 404
                };
            }
            _unitOfWork.Repository<Like>().Delete(like);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                Message = "Like added to the tweet successfully",
                IsSucceeded = true,
                Status = 201
            };
        }


        public async Task<ResponseDto> GetAllLikesOnTweetAsync(int tweetId)
        {
            var likes = await _unitOfWork.Repository<Like>().GetAllPredicated(t => t.TweetId == tweetId);

            if (likes == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "Likes not found"
                };
            }

            var mappedLikes = _mapper.Map<IEnumerable<GetLikeDto>>(likes);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Likes retrieved successfully",
                Models = mappedLikes
            };
        }
    }
}

