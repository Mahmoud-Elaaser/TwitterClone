using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.DTOs.FollowDto;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class FollowService : IFollowService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public FollowService(IUnitOfWork unitOfWork, IMapper mapper, INotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<ResponseDto> FollowUserAsync(CreateFollowDto createFollowDto)
        {
            var follower = await _unitOfWork.Repository<User>().GetByIdAsync(createFollowDto.FollowerId);
            var following = await _unitOfWork.Repository<User>().GetByIdAsync(createFollowDto.FollowingId);

            if (follower == null || following == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "Follower or following user not found."
                };
            }

            /// Check if the user is already following this user
            var existingFollow = await _unitOfWork.Repository<Follow>()
                .FindAsync(f => f.FollowerId == createFollowDto.FollowerId && f.FollowingId == createFollowDto.FollowingId);

            if (existingFollow != null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "You are already following this user."
                };
            }

            var follow = _mapper.Map<Follow>(createFollowDto);

            await _unitOfWork.Repository<Follow>().AddAsync(follow);
            await _unitOfWork.Complete();

            /// Notification: User [FollowerId] started following you
            await _notificationService.SendFollowNotificationAsync(follow.FollowerId, follow.FollowingId);

            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Followed successfully."
            };
        }

        public async Task<ResponseDto> UnfollowUserAsync(FollowDto followDto)
        {
            var followerId = followDto.FollowerId;
            var followingId = followDto.FollowingId;

            var follow = await _unitOfWork.Repository<Follow>()
                .FindAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "You are not following this user."
                };
            }

            _unitOfWork.Repository<Follow>().Delete(follow);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Unfollowed successfully."
            };
        }
        public async Task<ResponseDto> UnfollowUserAsync(int followerId, int followingId)
        {
            var follow = await _unitOfWork.Repository<Follow>().FindAsync(f => f.FollowerId == followerId && f.FollowingId == followingId);

            if (follow == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "Follow relationship does not exist."
                };
            }

            _unitOfWork.Repository<Follow>().Delete(follow);

            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Unfollowed successfully."
            };
        }

        public async Task<ResponseDto> GetFollowersAsync(int userId)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "User not found."
                };
            }

            var followers = await _unitOfWork.Repository<Follow>()
                .GetAllPredicated(f => f.FollowingId == userId, new[] { "Follower" });

            var followerDtos = _mapper.Map<IEnumerable<FollowerDto>>(followers);

            return new ResponseDto
            {
                IsSucceeded = true,
                Models = followerDtos
            };
        }

        public async Task<ResponseDto> GetFollowingAsync(int userId)
        {
            var user = await _unitOfWork.Repository<User>().GetByIdAsync(userId);
            if (user == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "User not found."
                };
            }

            var following = await _unitOfWork.Repository<Follow>()
                .GetAllPredicated(f => f.FollowerId == userId, new[] { "Following" });

            var followingDtos = following.Select(f => new FollowingDto
            {
                Id = f.Following.Id,
                Bio = f.Following.Bio
            });

            return new ResponseDto
            {
                IsSucceeded = true,
                Models = followingDtos
            };
        }
    }
}
