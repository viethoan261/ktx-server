using System.Collections.Generic;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Order;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IOrderService
    {
        Task<List<Order>> CreateOrders(List<OrderCreateRequestDTO> requests);
        Task<List<OrderResponseDTO>> GetOrdersByStudentId(int studentId);
        Task<List<OrderResponseDTO>> GetAllOrders();
        Task<OrderResponseDTO> GetOrderById(int id);
        Task<bool> UpdateOrderStatus(int id, string status);
        Task<bool> MarkOrderAsPaid(int id);
    }
} 