using System.Threading.Tasks;

namespace WebFilm.Core.Interfaces.Services
{
    public interface IStatisticsService
    {
        Task<object> GetRoomStatistics();
        Task<object> GetMonthlyBillStatistics(int? year = null);
    }
} 