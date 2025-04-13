using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.User;

namespace WebFilm.Core.Enitites.Room
{
    public class RoomResponseDTO
    {
        public int id { get; set; }
        
        public string floorNumber { get; set; }
        
        public string roomNumber { get; set; }
        
        public int maxOccupancy { get; set; }
        
        public string status { get; set; }
        
        public int currentOccupancy { get; set; }
        
        public DateTime? createdDate { get; set; }
        
        public DateTime? modifiedDate { get; set; }
        
        public List<Users> students { get; set; }
    }
} 