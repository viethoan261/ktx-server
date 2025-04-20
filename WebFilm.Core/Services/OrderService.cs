using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Enitites.Price;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPriceRepository _priceRepository;
        private readonly IRoomRepository _roomRepository;

        public OrderService(IOrderRepository orderRepository, IPriceRepository priceRepository, IRoomRepository roomRepository)
        {
            _orderRepository = orderRepository;
            _priceRepository = priceRepository;
            _roomRepository = roomRepository;
        }

        public async Task<List<Order>> CreateOrders(List<OrderCreateRequestDTO> requests)
        {
            // Get the latest price settings
            var prices = await _priceRepository.GetAll();
            var latestPrice = prices.OrderByDescending(p => p.id).FirstOrDefault();
            
            if (latestPrice == null)
            {
                throw new Exception("No price settings found. Please create price settings first.");
            }

            var result = new List<Order>();

            // Process each room request
            foreach (var request in requests)
            {
                // Get all rooms with students
                var roomsWithStudents = _roomRepository.GetAllRoomsWithStudents();
                var room = roomsWithStudents.FirstOrDefault(r => r.id == request.roomId);
                
                if (room == null)
                {
                    throw new Exception($"Room with ID {request.roomId} not found.");
                }

                // For each student in the room, create an order
                foreach (var student in room.students)
                {
                    // Calculate the number of students in the room
                    int studentCount = room.students.Count;
                    
                    // Calculate costs per student
                    decimal electricityCost = (request.electricNumberPerMonth * latestPrice.electricityPrice) / studentCount;
                    decimal waterCost = (request.waterNumberPerMonth * latestPrice.waterPrice) / studentCount;
                    
                    var order = new Order
                    {
                        studentId = student.id,
                        roomId = request.roomId,
                        electricNumberPerMonth = request.electricNumberPerMonth,
                        waterNumberPerMonth = request.waterNumberPerMonth,
                        electricity = electricityCost,
                        water = waterCost,
                        service = latestPrice.servicePrice,
                        room = latestPrice.roomPrice,
                        total = electricityCost + waterCost + latestPrice.servicePrice + latestPrice.roomPrice
                    };

                    await _orderRepository.Add(order);
                    result.Add(order);
                }
            }

            return result;
        }

        public async Task<List<OrderResponseDTO>> GetOrdersByStudentId(int studentId)
        {
            return await _orderRepository.GetOrdersByStudentId(studentId);
        }

        public async Task<List<OrderResponseDTO>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        public async Task<OrderResponseDTO> GetOrderById(int id)
        {
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new Exception($"Order with ID {id} not found.");
            }
            return order;
        }

        public async Task<bool> UpdateOrderStatus(int id, string status)
        {
            // Validate status
            if (string.IsNullOrEmpty(status))
            {
                throw new Exception("Status cannot be empty.");
            }
            
            // Check if order exists
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new Exception($"Order with ID {id} not found.");
            }
            
            // Update status
            var result = await _orderRepository.UpdateOrderStatus(id, status);
            return result > 0;
        }

        public async Task<bool> MarkOrderAsPaid(int id)
        {
            // Check if order exists
            var order = await _orderRepository.GetOrderById(id);
            if (order == null)
            {
                throw new Exception($"Order with ID {id} not found.");
            }
            
            // Check if order is already paid
            if (order.status == "Paid")
            {
                throw new Exception($"Order with ID {id} is already marked as paid.");
            }
            
            // Update status to Paid
            var result = await _orderRepository.UpdateOrderStatus(id, "Paid");
            return result > 0;
        }
    }
} 