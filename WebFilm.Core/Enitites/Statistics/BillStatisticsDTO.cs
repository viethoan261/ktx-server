using System.Collections.Generic;

namespace WebFilm.Core.Enitites.Statistics
{
    public class BillStatisticsDTO
    {
        public List<MonthlyBillStatistics> MonthlyStatistics { get; set; }
    }

    public class MonthlyBillStatistics
    {
        public int Month { get; set; }
        public int TotalBills { get; set; }
        public int PaidBills { get; set; }
        public int UnpaidBills { get; set; }
    }
} 