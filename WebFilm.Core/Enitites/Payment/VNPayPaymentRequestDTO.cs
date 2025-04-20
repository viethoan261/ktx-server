namespace WebFilm.Core.Enitites.Payment
{
    public class VNPayPaymentRequestDTO
    {
        public decimal Amount { get; set; }
        public string OrderDescription { get; set; }
        public string OrderType { get; set; } = "other";
        public string OrderId { get; set; }
        public string Name { get; set; }
        public string ReturnUrl { get; set; }
    }
} 