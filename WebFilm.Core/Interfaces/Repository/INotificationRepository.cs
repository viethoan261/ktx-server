using WebFilm.Core.Enitites.Notification;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface INotificationRepository : IBaseRepository<int, Notifications>
    {
        IEnumerable<Notifications> GetActiveNotifications();
        
        IEnumerable<Notifications> GetNotificationsByType(string type);
        
        int SetNotificationStatus(int id, string status);

        bool MarkNotificationAsRead(int userId, int notificationId);
        
        int MarkAllNotificationsAsRead(int userId);
        
        IEnumerable<NotificationReads> GetReadNotifications(int userId);
    }
} 