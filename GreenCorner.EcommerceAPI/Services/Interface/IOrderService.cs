using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Services.Interface
{
    public interface IOrderService
    {
        Task<List<OrderDTO>> GetAll();
        Task<OrderDTO> GetById(int id);
        Task Add(OrderDTO item);
        Task Update(OrderDTO item);
		Task UpdateOrderStatus(int orderId, string newStatus);
		Task Delete(int orderId);
        Task<List<OrderDTO>> GetByUserID(string userID);
        //dashboard
        Task<int> TotalOrdersComplete();
        Task<int> TotalOrdersWaiting();
        Task<int> TotalSales();
        Task<int> GetTotalMoneyByMonth();
        Task<List<BestSellingProductDTO>> GetBestSellingProduct();
        Task<MonthlyAnalyticsDto> GetMonthlySalesAnalytics(int year);
        Task<CategorySalesDto> GetSalesByCategory();
    }
}
