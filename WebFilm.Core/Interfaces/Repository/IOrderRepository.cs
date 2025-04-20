using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Order;

namespace WebFilm.Core.Interfaces.Repository
{
    public interface IOrderRepository
    {
        Task<int> Add(Order order);
        Task<List<OrderResponseDTO>> GetOrdersByStudentId(int studentId);
        Task<List<OrderResponseDTO>> GetAllOrders();
        Task<OrderResponseDTO> GetOrderById(int id);
        Task<int> UpdateOrderStatus(int id, string status);
    }
} 