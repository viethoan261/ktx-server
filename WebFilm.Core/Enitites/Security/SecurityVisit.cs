using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm.Core.Enitites.Security
{
    public class SecurityVisit : BaseEntity
    {
        #region Prop
        public string visitorName { get; set; }
        
        public string phoneNumber { get; set; }
        
        public int? studentId { get; set; }
        
        public DateTime entryTime { get; set; }
        
        public DateTime? exitTime { get; set; }
        
        public string status { get; set; } // "CHECKED_IN" or "CHECKED_OUT"
        
        public string purpose { get; set; }
        
        public string notes { get; set; }
        #endregion
    }
} 