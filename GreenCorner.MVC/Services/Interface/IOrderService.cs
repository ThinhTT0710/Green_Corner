using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IOrderService
    {
        Task<ResponseDTO?> GetAllOrder();
        Task<ResponseDTO?> GetOrderByID(int orderID);
        Task<ResponseDTO?> GetOrderByUserID(string userID);
        Task<ResponseDTO?> CreateOrder(OrderDTO orderDTO);
        Task<ResponseDTO?> UpdateOrder(OrderDTO orderDTO);
        Task<ResponseDTO?> DeleteOrder(int orderID);
		Task<ResponseDTO?> UpdateOrderStatus(int orderId, string orderStatus);
        Task<ResponseDTO?> TotalOrdersComplete();
        Task<ResponseDTO?> TotalOrdersWaiting();
		Task<ResponseDTO?> TotalSales();
        Task<ResponseDTO?> GetBestSellingProduct();
        Task<ResponseDTO?> GetTopBestSelling();
        Task<ResponseDTO?> GetTotalMoneyByMonth();
	}
}
