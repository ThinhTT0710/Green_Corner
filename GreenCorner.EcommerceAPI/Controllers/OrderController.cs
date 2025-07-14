using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EcommerceAPI.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ResponseDTO _responseDTO;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
            this._responseDTO = new ResponseDTO();
        }

        [HttpGet("get-orders")]
        public async Task<ResponseDTO> GetOrders()
        {
            try
            {
                var orders = await _orderService.GetAll();
                _responseDTO.Result = orders;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("get-order-by-id/{id}")]
        public async Task<ResponseDTO> GetOrderById(int id)
        {
            try
            {
                var order = await _orderService.GetById(id);
                _responseDTO.Result = order;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost("create-order")]
        public async Task<ResponseDTO> CreateOrder([FromBody] OrderDTO order)
        {
            try
            {
                await _orderService.Add(order);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut("update-order")]
        [Authorize]
        public async Task<ResponseDTO> UpdateOrder([FromBody] OrderDTO order)
        {
            try
            {
                await _orderService.Update(order);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

		[HttpPut("update-order-status")]
		public async Task<ResponseDTO> UpdateOrderStatus([FromBody] UpdateOrderStatusDTO updateOrderStatusDTO)
		{
			try
			{
				await _orderService.UpdateOrderStatus(updateOrderStatusDTO.OrderId, updateOrderStatusDTO.OrderStatus);
				return new ResponseDTO
				{
					Message = "Cập nhập thành công trạng thái của đơn hàng",
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				return new ResponseDTO
				{
					Message = ex.Message,
					IsSuccess = false
				};
			}
		}

		[HttpDelete("delete-order/{id}")]
        public async Task<ResponseDTO> DeleteOrder(int id)
        {
            try
            {
                await _orderService.Delete(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("get-order-by-user-id/{userId}")]
        public async Task<ResponseDTO> GetOrderByUserId(string userId)
        {
            try
            {
                var orders = await _orderService.GetByUserID(userId);
                _responseDTO.Result = orders;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        //dashboard
        [HttpGet("total-orders-complete")]
        public async Task<ResponseDTO> TotalOrdersComplete()
        {
            try
            {
                var total = await _orderService.TotalOrdersComplete();
                _responseDTO.Result = total;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("total-orders-waiting")]
        public async Task<ResponseDTO> TotalOrdersWaiting()
        {
            try
            {
                var total = await _orderService.TotalOrdersWaiting();
                _responseDTO.Result = total;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("total-sales")]
        public async Task<ResponseDTO> TotalSales()
        {
            try
            {
                var total = await _orderService.TotalSales();
                _responseDTO.Result = total;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpGet("total-money-by-month")]
        public async Task<ResponseDTO> GetTotalMoneyByMonth()
        {
            try
            {
                var total = await _orderService.GetTotalMoneyByMonth();
                _responseDTO.Result = total;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("get-best-selling-product")]
        public async Task<ResponseDTO> GetBestSeller()
        {
            try
            {
                var products = await _orderService.GetBestSellingProduct();
                _responseDTO.Result = products;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("get-top-selling-product")]
        public async Task<ResponseDTO> GetTrendingProduct()
        {
            try
            {
                var products = await _orderService.Top10BestSelling();
                _responseDTO.Result = products;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("monthly-analytics")]
        public async Task<ResponseDTO> GetMonthlyAnalytics()
        {
            try
            {
                // Lấy dữ liệu cho năm hiện tại
                int currentYear = DateTime.Now.Year;
                var analytics = await _orderService.GetMonthlySalesAnalytics(currentYear);
                _responseDTO.Result = analytics;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("sales-by-category")]
        public async Task<ResponseDTO> GetSalesByCategory()
        {
            try
            {
                var salesData = await _orderService.GetSalesByCategory();
                _responseDTO.Result = salesData;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
