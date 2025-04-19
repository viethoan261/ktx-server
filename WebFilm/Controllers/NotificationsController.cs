using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFilm.Core.Enitites.Notification;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BaseController<int, Notifications>
    {
        private readonly INotificationService _notificationService;
        private readonly IUserContext _userContext;

        public NotificationsController(INotificationService notificationService, IUserContext userContext)
            : base(notificationService)
        {
            _notificationService = notificationService;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetActiveNotifications()
        {
            try
            {
                int userId = _userContext.UserID;
                var notifications = _notificationService.GetNotificationsWithReadStatus(userId);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("type/{type}")]
        [Authorize]
        public IActionResult GetNotificationsByType(string type)
        {
            try
            {
                var notifications = _notificationService.GetNotificationsByType(type);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public IActionResult CreateNotification([FromBody] NotificationDTO notificationDTO)
        {
            try
            {
                int userId = _userContext.UserID;
                var notification = _notificationService.CreateNotification(notificationDTO, userId);
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult UpdateNotification(int id, [FromBody] NotificationDTO notificationDTO)
        {
            try
            {
                var notification = _notificationService.UpdateNotification(id, notificationDTO);
                if (notification == null)
                {
                    return NotFound($"Notification with ID {id} not found.");
                }
                return Ok(notification);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}/status")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult SetNotificationStatus(int id, [FromBody] string status)
        {
            try
            {
                var result = _notificationService.SetNotificationStatus(id, status);
                if (result == 0)
                {
                    return NotFound($"Notification with ID {id} not found.");
                }
                return Ok(new { message = $"Notification status updated to {status}." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public IActionResult DeleteNotification(int id)
        {
            try
            {
                var result = _notificationService.Delete(id);
                if (result == 0)
                {
                    return NotFound($"Notification with ID {id} not found.");
                }
                return Ok(new { message = "Notification deleted successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [HttpPost("{id}/read")]
        [Authorize]
        public IActionResult MarkAsRead(int id)
        {
            try
            {
                int userId = _userContext.UserID;
                var result = _notificationService.MarkNotificationAsRead(userId, id);
                
                if (!result)
                {
                    return NotFound($"Notification with ID {id} not found.");
                }
                
                return Ok(new { message = "Notification marked as read successfully." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        
        [HttpPost("read-all")]
        [Authorize]
        public IActionResult MarkAllAsRead()
        {
            try
            {
                int userId = _userContext.UserID;
                var count = _notificationService.MarkAllNotificationsAsRead(userId);
                
                return Ok(new { message = $"Marked {count} notifications as read." });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
} 