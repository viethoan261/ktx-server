using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Payment;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IVNPayService
    {
        string CreatePaymentUrl(HttpContext context, VNPayPaymentRequestDTO model);
        VNPayPaymentResponseDTO ProcessPaymentResponse(IQueryCollection collection);
        Task<Transaction> SaveTransaction(VNPayPaymentRequestDTO request, int userId);
        Task<Transaction> UpdateTransactionStatus(string orderId, string status, string transactionId);
        Task<Transaction> GetTransactionByOrderId(string orderId);
        Task<IEnumerable<Transaction>> GetTransactionsByUserId(int userId);
        Task<Transaction> UpdateTransactionIdAndDate(string orderId, string transactionId);
    }
} 