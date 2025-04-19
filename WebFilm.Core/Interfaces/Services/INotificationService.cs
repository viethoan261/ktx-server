using WebFilm.Core.Enitites.Notification;

namespace WebFilm.Core.Interfaces.Services
{
    public interface INotificationService : IBaseService<int, Notifications>
    {
        IEnumerable<NotificationResponseDTO> GetActiveNotifications();
        
        IEnumerable<NotificationResponseDTO> GetNotificationsByType(string type);
        
        NotificationResponseDTO CreateNotification(NotificationDTO notificationDTO, int userId);
        
        NotificationResponseDTO UpdateNotification(int id, NotificationDTO notificationDTO);
        
        int SetNotificationStatus(int id, string status);

        bool MarkNotificationAsRead(int userId, int notificationId);
        
        int MarkAllNotificationsAsRead(int userId);
        
        IEnumerable<NotificationResponseDTO> GetNotificationsWithReadStatus(int userId);
    }
} 