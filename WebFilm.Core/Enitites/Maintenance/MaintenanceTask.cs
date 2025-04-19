using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm.Core.Enitites.Maintenance
{
    [Table("maintenance_tasks")]
    public class MaintenanceTask : BaseEntity
    {
        #region Prop
        public string taskType { get; set; } // "MAINTENANCE" or "CLEANING"
        
        public string title { get; set; }
        
        public string description { get; set; }
        
        public string location { get; set; } // Room number or area
        
        public DateTime scheduledDate { get; set; }
        
        public DateTime? completedDate { get; set; }
        
        public string status { get; set; } // "SCHEDULED", "IN_PROGRESS", "COMPLETED", "CANCELLED"
        
        public string priority { get; set; } // "LOW", "MEDIUM", "HIGH", "URGENT"
        
        public string assignedTo { get; set; } // Staff ID/Name
        
        public string notes { get; set; }
        #endregion
    }
} 