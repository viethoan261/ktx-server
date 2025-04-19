using System;

namespace WebFilm.Core.Enitites.Security
{
    public class SecurityVisitResponseDTO
    {
        public int id { get; set; }
        
        public string visitorName { get; set; }
        
        public string phoneNumber { get; set; }
        
        public int? studentId { get; set; }
        
        public string studentName { get; set; }
        
        public DateTime entryTime { get; set; }
        
        public DateTime? exitTime { get; set; }
        
        public string status { get; set; }
        
        public string purpose { get; set; }
        
        public string notes { get; set; }
        
        public DateTime? createdDate { get; set; }
        
        public DateTime? modifiedDate { get; set; }
    }
} 