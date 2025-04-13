using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Request
{
    public class RequestResponseDTO
    {
        public string response { get; set; }
        
        public string status { get; set; } // "PENDING", "PROCESSING", "RESOLVED"
    }
} 