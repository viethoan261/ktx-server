using System;

namespace WebFilm.Core.Enitites.Notification
{
    public class NotificationDTO
    {
        public string title { get; set; }
        
        public string content { get; set; }
        
        public string type { get; set; } // "INTERNAL" or "EMERGENCY"
        
        public string status { get; set; } = "ACTIVE"; // Default to "ACTIVE"
        
        public DateTime? publishDate { get; set; }
        
        public DateTime? expiryDate { get; set; }
    }
} 