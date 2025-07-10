using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Admin;

namespace GreenCorner.MVC.ViewModels
{
	public class SaleAnalytics
	{
        public int TotalOrdersComplete { get; set; }
        public int TotalOrdersWaiting { get; set; }
		public int TotalSales { get; set; }
		public int TotalMoneyByMonth { get; set; }
        public MonthlyAnalyticsDto ChartData { get; set; }
        public List<BestSellingProductDTO> BestSellingProducts { get; set; }
        public List<ProductDTO> OutOfStockProduct { get; set; }
        public CategorySalesDto CategorySalesChartData { get; set; }
    }
}
