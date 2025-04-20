using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm.Core.Enitites.Order
{
    [Table("Orders")]
    public class Order : BaseEntity
    {
        public int studentId { get; set; }
        public int roomId { get; set; }
        public decimal electricity { get; set; }
        public decimal water { get; set; }
        public decimal service { get; set; }
        public decimal room { get; set; }
        public decimal total { get; set; }
        public string status { get; set; } = "Pending";
        public decimal electricNumberPerMonth { get; set; }
        public decimal waterNumberPerMonth { get; set; }
    }
} 