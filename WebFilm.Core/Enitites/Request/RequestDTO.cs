using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Request
{
    public class RequestDTO
    {
        public int studentId { get; set; }
        
        public string title { get; set; }
        
        public string content { get; set; }
        
        public string type { get; set; } // "REQUEST" or "COMPLAINT"
    }
} 