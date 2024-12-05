using TwitterClone.Service.DTOs;

namespace TwitterClone.Service.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ResponseDto> SendFollowNotificationAsync(int senderId, int receiverId);
        Task<ResponseDto> SendCommentNotificationAsync(int senderId, int tweetId);
        Task<ResponseDto> SendLikeNotificationAsync(int senderId, int tweetId);
        Task<ResponseDto> GetUserNotificationsAsync(int userId);
        Task<ResponseDto> getAllNotificationsAsync();
        Task<ResponseDto> MarkAllAsReadAsync(int userId);
    }

}
