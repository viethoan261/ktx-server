using System;

namespace WebFilm.Core.Enitites.Maintenance
{
    public class MaintenanceTaskResponseDTO
    {
        public int id { get; set; }
        
        public string taskType { get; set; }
        
        public string title { get; set; }
        
        public string description { get; set; }
        
        public string location { get; set; }
        
        public DateTime scheduledDate { get; set; }
        
        public DateTime? completedDate { get; set; }
        
        public string status { get; set; }
        
        public string priority { get; set; }
        
        public string assignedTo { get; set; }
        
        public string assignedToName { get; set; }
        
        public string notes { get; set; }
        
        public DateTime? createdDate { get; set; }
        
        public DateTime? modifiedDate { get; set; }
    }
} 