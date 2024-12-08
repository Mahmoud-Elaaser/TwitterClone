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



        [HttpGet("{userId}")]
        //[Authorize(Roles = "Admin")]
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

        [HttpPatch("mark-as-read")]
        public async Task<IActionResult> MarkNotificationAsRead([FromQuery] int notificationId)
        {
            var response = await _notificationService.MarkAsReadAsync(notificationId);

            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response.Message);
        }

        [HttpDelete("delete-notification")]
        public async Task<IActionResult> DeleteNotification([FromQuery] int notificationId)
        {
            var response = await _notificationService.DeleteNotificationById(notificationId);

            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response.Message);
        }

        [HttpGet("unread-notifications")]
        public async Task<IActionResult> GetUnreadNotifications(int userId)
        {
            var response = await _notificationService.GetUnreadNotifications(userId);

            if (!response.IsSucceeded)
            {
                return BadRequest(new ApiResponse(response.Status, response.Message));
            }

            return Ok(response.Models);
        }


    }

}