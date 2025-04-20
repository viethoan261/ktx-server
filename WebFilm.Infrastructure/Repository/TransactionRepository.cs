using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Payment;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class TransactionRepository : BaseRepository<int, Transaction>, ITransactionRepository
    {
        public TransactionRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> CreateTransaction(Transaction transaction)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                await SqlConnection.OpenAsync();
                var sqlCommand = @"
                    INSERT INTO transactions 
                    (OrderId, UserId, Amount, PaymentMethod, Status, OrderDescription, CreatedDate) 
                    VALUES 
                    (@OrderId, @UserId, @Amount, @PaymentMethod, @Status, @OrderDescription, @CreatedDate);
                    SELECT LAST_INSERT_ID();";
                
                var parameters = new DynamicParameters();
                parameters.Add("@OrderId", transaction.OrderId);
                parameters.Add("@UserId", transaction.UserId);
                parameters.Add("@Amount", transaction.Amount);
                parameters.Add("@PaymentMethod", transaction.PaymentMethod ?? "VNPay");
                parameters.Add("@Status", transaction.Status);
                parameters.Add("@OrderDescription", transaction.OrderDescription);
                parameters.Add("@CreatedDate", transaction.CreatedDate);
                
                var id = await SqlConnection.ExecuteScalarAsync<int>(sqlCommand, parameters);
                transaction.Id = id;
                return id;
            }
        }

        public async Task<Transaction> GetByOrderId(string orderId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                await SqlConnection.OpenAsync();
                var sqlCommand = @"
                    SELECT * FROM transactions 
                    WHERE OrderId = @OrderId";
                
                var parameters = new DynamicParameters();
                parameters.Add("@OrderId", orderId);
                
                var transaction = await SqlConnection.QueryFirstOrDefaultAsync<Transaction>(sqlCommand, parameters);
                return transaction;
            }
        }

        public async Task<IEnumerable<Transaction>> GetByUserId(int userId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                await SqlConnection.OpenAsync();
                var sqlCommand = @"
                    SELECT * FROM transactions 
                    WHERE UserId = @UserId
                    ORDER BY CreatedDate DESC";
                
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                
                var transactions = await SqlConnection.QueryAsync<Transaction>(sqlCommand, parameters);
                return transactions;
            }
        }

        public async Task<int> UpdateTransactionStatus(string orderId, string status, string transactionId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                await SqlConnection.OpenAsync();
                var sqlCommand = @"
                    UPDATE transactions 
                    SET Status = @Status, 
                        TransactionId = @TransactionId,
                        CompletedDate = CASE WHEN @Status = 'SUCCESS' THEN @CompletedDate ELSE NULL END
                    WHERE OrderId = @OrderId";
                
                var parameters = new DynamicParameters();
                parameters.Add("@Status", status);
                parameters.Add("@TransactionId", transactionId);
                parameters.Add("@OrderId", orderId);
                parameters.Add("@CompletedDate", DateTime.Now);
                
                var result = await SqlConnection.ExecuteAsync(sqlCommand, parameters);
                return result;
            }
        }

        public async Task<int> UpdateTransactionIdAndDate(string orderId, string transactionId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                await SqlConnection.OpenAsync();
                var sqlCommand = @"
                    UPDATE transactions 
                    SET TransactionId = @TransactionId,
                        CompletedDate = @CompletedDate,
                        Status = 'SUCCESS'
                    WHERE OrderId = @OrderId";
                
                var parameters = new DynamicParameters();
                parameters.Add("@TransactionId", transactionId);
                parameters.Add("@OrderId", orderId);
                parameters.Add("@CompletedDate", DateTime.Now);
                
                var result = await SqlConnection.ExecuteAsync(sqlCommand, parameters);
                return result;
            }
        }
    }
} 