using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm.Core.Enitites.Notification
{
    public class Notifications : BaseEntity
    {
        #region Prop
        public string title { get; set; }
        
        public string content { get; set; }
        
        public string type { get; set; } // "INTERNAL" or "EMERGENCY"
        
        public string status { get; set; } // "ACTIVE" or "INACTIVE"
        
        public DateTime? publishDate { get; set; }
        
        public DateTime? expiryDate { get; set; }

        public int? createdBy { get; set; } // Reference to Users.id
        #endregion
    }
} 