using System.ComponentModel.DataAnnotations.Schema;

namespace WebFilm.Core.Enitites.Price
{
    [Table("Price")]
    public class Price : BaseEntity
    {
        public decimal electricityPrice { get; set; }
        public decimal waterPrice { get; set; }
        public decimal servicePrice { get; set; }
        public decimal roomPrice { get; set; }
    }
} 