using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Room
{
    public class RoomDTO
    {
        public string floorNumber { get; set; }
        
        public string roomNumber { get; set; }
        
        public int maxOccupancy { get; set; }
        
        public string status { get; set; }
        
        public int currentOccupancy { get; set; }
        
        public List<int> studentIds { get; set; }
    }
} 