using System;

namespace WebFilm.Core.Enitites.Security
{
    public class SecurityVisitDTO
    {
        public string visitorName { get; set; }
        
        public string phoneNumber { get; set; }
        
        public int? studentId { get; set; }
        
        public string purpose { get; set; }
        
        public string notes { get; set; }
    }
} 