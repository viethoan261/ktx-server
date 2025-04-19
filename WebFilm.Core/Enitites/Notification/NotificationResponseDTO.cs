using System;

namespace WebFilm.Core.Enitites.Notification
{
    public class NotificationResponseDTO
    {
        public int id { get; set; }
        
        public string title { get; set; }
        
        public string content { get; set; }
        
        public string type { get; set; }
        
        public string status { get; set; }
        
        public DateTime? publishDate { get; set; }
        
        public DateTime? expiryDate { get; set; }
        
        public int? createdBy { get; set; }
        
        public string createdByName { get; set; }
        
        public DateTime? createdDate { get; set; }
        
        public DateTime? modifiedDate { get; set; }
        
        public bool isRead { get; set; } = false;
        
        public DateTime? readAt { get; set; }
    }
} 