using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebFilm.Core.Enitites.Order;
using WebFilm.Core.Enitites.Room;
using WebFilm.Core.Enitites.Statistics;
using WebFilm.Core.Interfaces.Repository;
using WebFilm.Core.Interfaces.Services;

namespace WebFilm.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IOrderRepository _orderRepository;

        public StatisticsService(IRoomRepository roomRepository, IOrderRepository orderRepository)
        {
            _roomRepository = roomRepository;
            _orderRepository = orderRepository;
        }

        public async Task<object> GetRoomStatistics()
        {
            var rooms = _roomRepository.GetAll(); 

            var statusCounts = rooms
                .GroupBy(r => r.status ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());

            var roomsOccupancy = rooms.Select(r => new RoomOccupancyDTO
            {
                RoomId = r.id,
                RoomNumber = r.roomNumber,
                FloorNumber = r.floorNumber,
                Status = r.status,
                MaxOccupancy = r.maxOccupancy,
                CurrentOccupancy = r.currentOccupancy
            }).ToList();

            return await Task.FromResult(new RoomStatisticsDTO
            {
                StatusCounts = statusCounts,
                RoomsOccupancy = roomsOccupancy
            });
        }

        public async Task<object> GetMonthlyBillStatistics(int? year = null)
        {
            var ordersDto = await _orderRepository.GetAllOrders();

            IEnumerable<OrderResponseDTO> filteredOrders = ordersDto;
            if (year.HasValue)
            {
                filteredOrders = ordersDto.Where(o => o.createdDate.HasValue && o.createdDate.Value.Year == year.Value);
            }

            var monthlyStats = filteredOrders
                .Where(o => o.createdDate.HasValue)
                .GroupBy(o => o.createdDate.Value.Month)
                .Select(g => new MonthlyBillStatistics
                {
                    Month = g.Key,
                    TotalBills = g.Count(),
                    PaidBills = g.Count(o => (o.status ?? "Pending").Equals("Paid", StringComparison.OrdinalIgnoreCase)),
                    UnpaidBills = g.Count(o => !(o.status ?? "Pending").Equals("Paid", StringComparison.OrdinalIgnoreCase))
                })
                .OrderBy(s => s.Month)
                .ToList();

            var result = new List<MonthlyBillStatistics>();
            for (int month = 1; month <= 12; month++)
            {
                var monthStat = monthlyStats.FirstOrDefault(s => s.Month == month);
                result.Add(monthStat ?? new MonthlyBillStatistics
                {
                    Month = month,
                    TotalBills = 0,
                    PaidBills = 0,
                    UnpaidBills = 0
                });
            }

            return new BillStatisticsDTO
            {
                MonthlyStatistics = result
            };
        }
    }
} 