using WebFilm.Core.Enitites.Notification;
using WebFilm.Core.Enitites.User;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class NotificationService : BaseService<int, Notifications>, INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;

        public NotificationService(INotificationRepository notificationRepository, IUserRepository userRepository) 
            : base(notificationRepository)
        {
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<NotificationResponseDTO> GetActiveNotifications()
        {
            var notifications = _notificationRepository.GetActiveNotifications();
            return MapToResponseDTOs(notifications);
        }

        public IEnumerable<NotificationResponseDTO> GetNotificationsByType(string type)
        {
            var notifications = _notificationRepository.GetNotificationsByType(type);
            return MapToResponseDTOs(notifications);
        }

        public NotificationResponseDTO CreateNotification(NotificationDTO notificationDTO, int userId)
        {
            var notification = new Notifications
            {
                title = notificationDTO.title,
                content = notificationDTO.content,
                type = notificationDTO.type,
                status = notificationDTO.status,
                publishDate = notificationDTO.publishDate ?? DateTime.Now,
                expiryDate = notificationDTO.expiryDate,
                createdBy = userId,
                createdDate = DateTime.Now,
                modifiedDate = DateTime.Now
            };

            _notificationRepository.Add(notification);
            return MapToResponseDTO(notification);
        }

        public NotificationResponseDTO UpdateNotification(int id, NotificationDTO notificationDTO)
        {
            var existingNotification = _notificationRepository.GetByID(id);
            if (existingNotification == null)
            {
                return null;
            }

            existingNotification.title = notificationDTO.title;
            existingNotification.content = notificationDTO.content;
            existingNotification.type = notificationDTO.type;
            existingNotification.status = notificationDTO.status;
            existingNotification.publishDate = notificationDTO.publishDate;
            existingNotification.expiryDate = notificationDTO.expiryDate;
            existingNotification.modifiedDate = DateTime.Now;

            _notificationRepository.Edit(id, existingNotification);
            return MapToResponseDTO(existingNotification);
        }

        public int SetNotificationStatus(int id, string status)
        {
            return _notificationRepository.SetNotificationStatus(id, status);
        }

        public bool MarkNotificationAsRead(int userId, int notificationId)
        {
            return _notificationRepository.MarkNotificationAsRead(userId, notificationId);
        }
        
        public int MarkAllNotificationsAsRead(int userId)
        {
            return _notificationRepository.MarkAllNotificationsAsRead(userId);
        }
        
        public IEnumerable<NotificationResponseDTO> GetNotificationsWithReadStatus(int userId)
        {
            var notifications = _notificationRepository.GetActiveNotifications();
            var readNotifications = _notificationRepository.GetReadNotifications(userId);
            
            var readNotificationDict = readNotifications.ToDictionary(
                r => r.notification_id,
                r => r
            );
            
            var responseDTOs = new List<NotificationResponseDTO>();
            
            foreach (var notification in notifications)
            {
                var dto = MapToResponseDTO(notification);
                
                if (readNotificationDict.TryGetValue(notification.id, out var readInfo))
                {
                    dto.isRead = true;
                    dto.readAt = readInfo.read_at;
                }
                
                responseDTOs.Add(dto);
            }
            
            return responseDTOs;
        }

        private IEnumerable<NotificationResponseDTO> MapToResponseDTOs(IEnumerable<Notifications> notifications)
        {
            var responseDTOs = new List<NotificationResponseDTO>();
            
            foreach (var notification in notifications)
            {
                responseDTOs.Add(MapToResponseDTO(notification));
            }
            
            return responseDTOs;
        }

        private NotificationResponseDTO MapToResponseDTO(Notifications notification)
        {
            string createdByName = null;
            if (notification.createdBy.HasValue)
            {
                var user = _userRepository.GetByID(notification.createdBy.Value);
                createdByName = user?.fullname;
            }
            
            return new NotificationResponseDTO
            {
                id = notification.id,
                title = notification.title,
                content = notification.content,
                type = notification.type,
                status = notification.status,
                publishDate = notification.publishDate,
                expiryDate = notification.expiryDate,
                createdBy = notification.createdBy,
                createdByName = createdByName,
                createdDate = notification.createdDate,
                modifiedDate = notification.modifiedDate
            };
        }
    }
} 