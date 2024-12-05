using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterClone.Api.Response;
using TwitterClone.Service.DTOs;
using TwitterClone.Service.Services.Interfaces;

namespace TwitterClone.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAllAsync()
        {
            var response = await _notificationService.getAllNotificationsAsync();
            return Ok(response.Models);
        }

        [HttpPost("follow")]
        public async Task<IActionResult> SendFollowNotification(int senderId, int receiverId)
        {
            var response = await _notificationService.SendFollowNotificationAsync(senderId, receiverId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Model);
        }

        [HttpPost("comment")]
        public async Task<IActionResult> SendCommentNotification(int senderId, int tweetId)
        {
            var response = await _notificationService.SendCommentNotificationAsync(senderId, tweetId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Model);
        }

        [HttpPost("like")]
        public async Task<IActionResult> SendLikeNotification(int senderId, int tweetId)
        {
            var response = await _notificationService.SendLikeNotificationAsync(senderId, tweetId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Model);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserNotifications(int userId)
        {
            var response = await _notificationService.GetUserNotificationsAsync(userId);
            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }
            return Ok(response.Models);
        }

        [HttpPatch("mark-all-as-read")] /// just update isRead property so, used Patch
        public async Task<IActionResult> MarkAllAsRead([FromQuery] int userId)
        {
            var response = await _notificationService.MarkAllAsReadAsync(userId);

            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response.Message);
        }

    }

}


/*
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("unread")]
        public async Task<IActionResult> GetUnreadNotifications()
        {
            int userId = GetCurrentUserId();
            var notifications = await _notificationService.GetNotificationsForUserAsync(userId);
            return Ok(notifications);
        }

        [HttpPatch("{notificationId}/mark-as-read")]
        public async Task<IActionResult> MarkNotificationAsRead(int notificationId)
        {
            await _notificationService.MarkAsReadAsync(notificationId);
            return NoContent();
        }

        [HttpPatch("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            int userId = GetCurrentUserId();
            await _notificationService.MarkAsReadAsync(userId);
            return NoContent();
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllNotifications()
        {
            int userId = GetCurrentUserId();
            var notifications = await _notificationService.GetNotificationsForUserAsync(userId);
            return Ok(notifications);
        }

        [HttpDelete("{notificationId}")]
        public async Task<IActionResult> DeleteNotification(int notificationId)
        {
            await _notificationService.DeleteNotificationAsync(notificationId);
            return NoContent();
        }



        [HttpDelete("delete-all")]
        public async Task<IActionResult> DeleteAllNotifications()
        {
            int userId = GetCurrentUserId();
            await _notificationService.DeleteAllNotificationsForUserAsync(userId);
            return NoContent();
        }


        private int GetCurrentUserId()
        {
            return int.Parse(User.FindFirst("Id")?.Value);
        }
    }
    */
