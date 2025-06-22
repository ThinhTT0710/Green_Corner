using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseService _baseService;
        public OrderService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> GetOrderByID(int orderID)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/order/get-order-by-id/" + orderID
            });
        }

        public async Task<ResponseDTO?> GetOrderByUserID(string userId)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/order/get-order-by-user-id/" + userId
            });
        }

        public async Task<ResponseDTO?> CreateOrder(OrderDTO orderDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.EcommerceAPIBase + "/api/order/create-order",
                Data = orderDTO
            });
        }

        public async Task<ResponseDTO?> GetAllOrder()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/order/get-orders"
            });
        }

        public async Task<ResponseDTO?> UpdateOrder(OrderDTO orderDTO)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Url = SD.EcommerceAPIBase + "/api/order/update-order",
                Data = orderDTO
            });
        }

        public async Task<ResponseDTO?> DeleteOrder(int orderID)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EcommerceAPIBase + "/api/order/delete-order/" + orderID
            });
        }

		public async Task<ResponseDTO?> UpdateOrderStatus(int orderId, string orderStatus)
		{
			var requestData = new
			{
				OrderId = orderId,
				OrderStatus = orderStatus
			};

			return await _baseService.SendAsync(new RequestDTO
			{
				APIType = SD.APIType.PUT,
				Url = SD.EcommerceAPIBase + "/api/order/update-order-status",
				Data = requestData
			});
		}
	}
}
