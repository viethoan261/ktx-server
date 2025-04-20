using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Payment;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface ITransactionRepository : IBaseRepository<int, Transaction>
    {
        Task<Transaction> GetByOrderId(string orderId);
        Task<IEnumerable<Transaction>> GetByUserId(int userId);
        Task<int> CreateTransaction(Transaction transaction);
        Task<int> UpdateTransactionStatus(string orderId, string status, string transactionId);
        Task<int> UpdateTransactionIdAndDate(string orderId, string transactionId);
    }
} 