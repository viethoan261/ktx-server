using Dapper;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Interfaces.Repository;

namespace WebFilm.Infrastructure.Repository
{
    public class OrderRepository : BaseRepository<int, Order>, IOrderRepository
    {
        public OrderRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<int> Add(Order order)
        {
            return await Task.FromResult(base.Add(order));
        }

        public async Task<List<OrderResponseDTO>> GetOrdersByStudentId(int studentId)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT o.*, u.fullname AS studentName, r.roomNumber
                    FROM `Order` o
                    JOIN Users u ON o.studentId = u.id
                    JOIN rooms r ON o.roomId = r.id
                    WHERE o.studentId = @v_StudentId
                    ORDER BY o.createdDate DESC";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_StudentId", studentId);
                
                var orders = SqlConnection.Query<OrderResponseDTO>(sqlCommand, parameters).ToList();
                SqlConnection.Close();
                
                return await Task.FromResult(orders);
            }
        }

        public async Task<List<OrderResponseDTO>> GetAllOrders()
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT o.*, u.fullname AS studentName, r.roomNumber
                    FROM `Order` o
                    JOIN Users u ON o.studentId = u.id
                    JOIN rooms r ON o.roomId = r.id
                    ORDER BY o.createdDate DESC";
                
                var orders = SqlConnection.Query<OrderResponseDTO>(sqlCommand).ToList();
                SqlConnection.Close();
                
                return await Task.FromResult(orders);
            }
        }

        public async Task<OrderResponseDTO> GetOrderById(int id)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    SELECT o.*, u.fullname AS studentName, r.roomNumber
                    FROM `Order` o
                    JOIN Users u ON o.studentId = u.id
                    JOIN rooms r ON o.roomId = r.id
                    WHERE o.id = @v_OrderId";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_OrderId", id);
                
                var order = SqlConnection.QueryFirstOrDefault<OrderResponseDTO>(sqlCommand, parameters);
                SqlConnection.Close();
                
                return await Task.FromResult(order);
            }
        }

        public async Task<int> UpdateOrderStatus(int id, string status)
        {
            using (SqlConnection = new MySqlConnection(_connectionString))
            {
                var sqlCommand = @"
                    UPDATE `Order`
                    SET status = @v_Status, modifiedDate = NOW()
                    WHERE id = @v_OrderId";
                
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("v_OrderId", id);
                parameters.Add("v_Status", status);
                
                var result = SqlConnection.Execute(sqlCommand, parameters);
                SqlConnection.Close();
                
                return await Task.FromResult(result);
            }
        }
    }
} 