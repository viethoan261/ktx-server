using System;

namespace WebFilm.Core.Enitites.Order
{
    public class OrderResponseDTO
    {
        public int id { get; set; }
        public int studentId { get; set; }
        public string studentName { get; set; }
        public int roomId { get; set; }
        public string roomNumber { get; set; }
        public decimal electricity { get; set; }
        public decimal water { get; set; }
        public decimal service { get; set; }
        public decimal room { get; set; }
        public decimal total { get; set; }
        public string status { get; set; }
        public decimal electricNumberPerMonth { get; set; }
        public decimal waterNumberPerMonth { get; set; }
        public DateTime? createdDate { get; set; }
        public DateTime? modifiedDate { get; set; }
    }

    public class OrderStatusUpdateDTO
    {
        public string status { get; set; }
    }
} 