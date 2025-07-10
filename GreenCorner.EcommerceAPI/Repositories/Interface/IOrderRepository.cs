using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Repositories.Interface
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAll();
        Task<Order> GetById(int id);
        Task Add(Order order);
        Task Update(Order order);
		Task UpdateOrderStatus(int orderId, string newStatus);
		Task Delete(int id);
        Task<IEnumerable<Order>> GetByUserId(string userId);
        //dashboard
        Task<int> TotalOrdersComplete();
        Task<int> TotalOrdersWaiting();
        Task<int> TotalSales();
        Task<int> GetTotalMoneyByMonth();
        Task<MonthlyAnalyticsDto> GetMonthlySalesAnalytics(int year);
        Task<CategorySalesDto> GetSalesByCategory();
    }
}
