using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Request
{
    public class Request : BaseEntity
    {
        public int studentId { get; set; }
        
        public string title { get; set; }
        
        public string content { get; set; }
        
        public string type { get; set; } // "REQUEST" or "COMPLAINT"
        
        public string status { get; set; } // "PENDING", "PROCESSING", "RESOLVED"
        
        public string response { get; set; }
        
        public DateTime? resolvedDate { get; set; }
    }
} 