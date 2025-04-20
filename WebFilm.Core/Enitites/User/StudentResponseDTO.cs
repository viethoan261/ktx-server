using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.User
{
    public class StudentResponseDTO
    {
        public int id { get; set; }
        
        public string username { get; set; }
        
        public string password { get; set; }
        
        public string fullname { get; set; }
        
        public string email { get; set; }
        
        public string phone { get; set; }
        
        public int? roomId { get; set; }
        
        public string roomNumber { get; set; }
    }
} 