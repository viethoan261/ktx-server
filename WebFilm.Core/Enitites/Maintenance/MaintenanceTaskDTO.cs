using System;

namespace WebFilm.Core.Enitites.Maintenance
{
    public class MaintenanceTaskDTO
    {
        public string taskType { get; set; } // "MAINTENANCE" or "CLEANING"
        
        public string title { get; set; }
        
        public string description { get; set; }
        
        public string location { get; set; } // Room number or area
        
        public DateTime scheduledDate { get; set; }
        
        public string priority { get; set; } // "LOW", "MEDIUM", "HIGH", "URGENT"
        
        public string assignedTo { get; set; } // Staff ID/Name
        
        public string notes { get; set; }
    }
} 