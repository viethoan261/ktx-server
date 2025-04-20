using System.Collections.Generic;

namespace WebFilm.Core.Enitites.Statistics
{
    public class RoomStatisticsDTO
    {
        public Dictionary<string, int> StatusCounts { get; set; }
        public List<RoomOccupancyDTO> RoomsOccupancy { get; set; }
    }

    public class RoomOccupancyDTO
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public string FloorNumber { get; set; }
        public string Status { get; set; }
        public int MaxOccupancy { get; set; }
        public int CurrentOccupancy { get; set; }
    }
} 