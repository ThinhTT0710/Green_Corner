using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Admin;

namespace GreenCorner.MVC.ViewModels
{
    public class AdminViewModel
    {
        public int TotalOrdersComplete { get; set; }
        public int TotalOrdersWaiting { get; set; }
        public int TotalSales { get; set; }
        public int TotalMoneyByMonth { get; set; }
        public int TotalEvent { get; set; }
        public int TotalOpenEvent { get; set; }
        public int TotalVolunteer { get; set; }
        public int TotalPedingVolunteer { get; set; }
        public MonthlyAnalyticsDto ChartData { get; set; }
        public List<BestSellingProductDTO> BestSellingProducts { get; set; }
        public List<ProductDTO> OutOfStockProduct { get; set; }
        public CategorySalesDto CategorySalesChartData { get; set; }
        public List<EventDTO> EventOpenList { get; set; }
        public List<TrashEventDTO> TrashEventList { get; set; }
        public EventMonthlyAnalyticsDto ChartData2 { get; set; }
    }
}
