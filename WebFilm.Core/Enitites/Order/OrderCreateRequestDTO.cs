using System.Collections.Generic;

namespace WebFilm.Core.Enitites.Order
{
    public class OrderCreateRequestDTO
    {
        public int roomId { get; set; }
        public decimal electricNumberPerMonth { get; set; }
        public decimal waterNumberPerMonth { get; set; }
    }

    public class OrderCreateRequest
    {
        public List<OrderCreateRequestDTO> orders { get; set; }
    }
} 