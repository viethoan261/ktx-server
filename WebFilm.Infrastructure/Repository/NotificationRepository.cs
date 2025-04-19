using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using WebFilm.Core.Enitites.Notification;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class NotificationRepository : BaseRepository<int, Notifications>, INotificationRepository
    {
        public NotificationRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<Notifications> GetActiveNotifications()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `Notifications` 
                    WHERE status = 'ACTIVE' 
                    AND (publishDate IS NULL OR publishDate <= NOW()) 
                    AND (expiryDate IS NULL OR expiryDate >= NOW())
                    ORDER BY publishDate DESC";
                
                var notifications = SqlConnection.Query<Notifications>(sqlCommand);
                SqlConnection.Close();
                return notifications;
            }
        }

        public IEnumerable<Notifications> GetNotificationsByType(string type)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `Notifications` 
                    WHERE type = @type 
                    ORDER BY publishDate DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@type", type);
                
                var notifications = SqlConnection.Query<Notifications>(sqlCommand, parameters);
                SqlConnection.Close();
                return notifications;
            }
        }

        public int SetNotificationStatus(int id, string status)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    UPDATE `Notifications` 
                    SET status = @status, modifiedDate = NOW() 
                    WHERE id = @id";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                parameters.Add("@status", status);
                
                var result = SqlConnection.Execute(sqlCommand, parameters);
                SqlConnection.Close();
                return result;
            }
        }

        public bool MarkNotificationAsRead(int userId, int notificationId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                // Check if the notification exists
                var checkNotificationSql = "SELECT COUNT(1) FROM `Notifications` WHERE id = @notificationId";
                var notificationExists = SqlConnection.ExecuteScalar<int>(checkNotificationSql, new { notificationId }) > 0;
                
                if (!notificationExists)
                {
                    return false;
                }
                
                // Check if already marked as read
                var checkSql = @"
                    SELECT COUNT(1) FROM `notification_reads` 
                    WHERE user_id = @userId AND notification_id = @notificationId";
                
                var alreadyRead = SqlConnection.ExecuteScalar<int>(checkSql, new { userId, notificationId }) > 0;
                
                if (alreadyRead)
                {
                    return true; // Already marked as read
                }
                
                // Mark as read
                var insertSql = @"
                    INSERT INTO `notification_reads` (user_id, notification_id, read_at) 
                    VALUES (@userId, @notificationId, NOW())";
                
                var result = SqlConnection.Execute(insertSql, new { userId, notificationId });
                
                SqlConnection.Close();
                return result > 0;
            }
        }
        
        public int MarkAllNotificationsAsRead(int userId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                // Get all active notifications not yet read by the user
                var selectSql = @"
                    SELECT n.id FROM `Notifications` n
                    LEFT JOIN `notification_reads` nr ON n.id = nr.notification_id AND nr.user_id = @userId
                    WHERE nr.id IS NULL
                    AND n.status = 'ACTIVE'
                    AND (n.publishDate IS NULL OR n.publishDate <= NOW())
                    AND (n.expiryDate IS NULL OR n.expiryDate >= NOW())";
                
                var unreadNotificationIds = SqlConnection.Query<int>(selectSql, new { userId }).ToList();
                
                if (!unreadNotificationIds.Any())
                {
                    return 0; // No unread notifications
                }
                
                // Insert read records for all unread notifications
                var insertSql = @"
                    INSERT INTO `notification_reads` (user_id, notification_id, read_at)
                    VALUES (@userId, @notificationId, NOW())";
                
                var insertCount = 0;
                foreach (var notificationId in unreadNotificationIds)
                {
                    insertCount += SqlConnection.Execute(insertSql, new { userId, notificationId });
                }
                
                SqlConnection.Close();
                return insertCount;
            }
        }
        
        public IEnumerable<NotificationReads> GetReadNotifications(int userId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT * FROM `notification_reads` 
                    WHERE user_id = @userId";
                
                var parameters = new DynamicParameters();
                parameters.Add("@userId", userId);
                
                var readNotifications = SqlConnection.Query<NotificationReads>(sqlCommand, parameters);
                SqlConnection.Close();
                return readNotifications;
            }
        }
    }
} 