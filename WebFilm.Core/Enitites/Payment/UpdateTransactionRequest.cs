namespace WebFilm.Core.Enitites.Payment
{
    public class UpdateTransactionRequest
    {
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
    }
} 