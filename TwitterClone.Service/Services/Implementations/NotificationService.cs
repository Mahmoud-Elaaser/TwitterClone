using AutoMapper;
using TwitterClone.Data.Entities;
using TwitterClone.Data.Enums;
using TwitterClone.Infrastructure.Repositories.Interfaces;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Service.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseDto> SendFollowNotificationAsync(int senderId, int receiverId)
        {
            var notification = new Notification
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                Type = NotificationType.NewFollower,
                Message = $"User with id: {senderId} started following you",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.Complete();

            var mappedNotification = _mapper.Map<NotificationDto>(notification);
            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Follow's notification sent IsSucceededfully.",
                Model = mappedNotification
            };
        }

        public async Task<ResponseDto> SendCommentNotificationAsync(int senderId, int tweetId)
        {
            var tweet = await _unitOfWork.Repository<Tweet>().GetByIdAsync(tweetId);
            if (tweet == null)
            {
                return new ResponseDto { IsSucceeded = false, Message = "Tweet not found." };
            }

            var notification = new Notification
            {
                SenderId = senderId,
                ReceiverId = tweet.UserId,
                Type = NotificationType.Comment,
                Message = $"User with id: {senderId} commented on your tweet.",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.Complete();

            var mappedNotification = _mapper.Map<NotificationDto>(notification);
            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Comment's notification sent succeededfully.",
                Model = mappedNotification
            };
        }

        public async Task<ResponseDto> SendLikeNotificationAsync(int senderId, int tweetId)
        {
            var tweet = await _unitOfWork.Repository<Tweet>().GetByIdAsync(tweetId);
            if (tweet == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Message = "Tweet not found."
                };
            }

            var notification = new Notification
            {
                SenderId = senderId,
                ReceiverId = tweet.UserId,
                Type = NotificationType.Like,
                Message = $"User with id: {senderId} liked your tweet.",
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<Notification>().AddAsync(notification);
            await _unitOfWork.Complete();

            var mappedNotification = _mapper.Map<NotificationDto>(notification);
            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "Like's notification sent succeededfully.",
                Model = mappedNotification
            };
        }

        /// retrieve all users's notifications
        public async Task<ResponseDto> GetUserNotificationsAsync(int userId)
        {
            var notifications = await _unitOfWork.Repository<Notification>()
                .GetAllPredicated(n => n.ReceiverId == userId);
            if (notifications == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "No notifications were sent yet."
                };
            }

            var notificationDtos = _mapper.Map<IEnumerable<NotificationDto>>(notifications);

            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Models = notificationDtos
            };
        }


        public async Task<ResponseDto> getAllNotificationsAsync()
        {
            var notifications = await _unitOfWork.Repository<Notification>().GetAllAsync();
            if (notifications == null)
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "No notifications were sent yet."
                };
            }
            var mappedNotifications = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            return new ResponseDto
            {
                IsSucceeded = true,
                Status = 200,
                Message = "Here are all notifications",
                Models = mappedNotifications
            };
        }

        public async Task<ResponseDto> MarkAllAsReadAsync(int userId)
        {
            var notifications = await _unitOfWork.Repository<Notification>().GetAllPredicated(n => n.ReceiverId == userId && !n.IsRead);

            if (!notifications.Any())
            {
                return new ResponseDto
                {
                    IsSucceeded = false,
                    Status = 404,
                    Message = "No unread notifications to mark as read."
                };
            }

            /// iterate over all notifications for updating all notifications to be read [isRead = true]
            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _unitOfWork.Repository<Notification>().UpdateRangeAsync((ICollection<Notification>)notifications);
            await _unitOfWork.Complete();

            return new ResponseDto
            {
                IsSucceeded = true,
                Message = "All notifications have been marked as read."
            };
        }
    }

}
