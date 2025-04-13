using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFilm.Core.Enitites.Room
{
    public class RoomStudent : BaseEntity
    {
        public int roomId { get; set; }
        
        public int studentId { get; set; }
    }
} 