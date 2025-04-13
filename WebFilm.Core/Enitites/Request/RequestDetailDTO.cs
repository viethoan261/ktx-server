using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Enitites.Request
{
    public class RequestDetailDTO
    {
        public int id { get; set; }
        
        public int studentId { get; set; }
        
        public string studentName { get; set; }
        
        public string title { get; set; }
        
        public string content { get; set; }
        
        public string type { get; set; }
        
        public string status { get; set; }
        
        public string response { get; set; }
        
        public DateTime createdDate { get; set; }
        
        public DateTime? modifiedDate { get; set; }
        
        public DateTime? resolvedDate { get; set; }
    }
} 