using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
	public class SaleStaffController : Controller
	{
		private IOrderService _orderService;
        private readonly IProductService _productService;
        public SaleStaffController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public IActionResult Index()
		{
			return View();
		}

        public async Task<IActionResult> ViewAllProduct()
        {
            List<ProductDTO> listProduct = new();
            ResponseDTO? response = await _productService.GetAllProduct();
            if (response != null && response.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listProduct);
        }

        public async Task<IActionResult> ProductDetails(int productId)
        {
            ProductDTO product = new();
            var response = await _productService.GetByProductId(productId);

            if (response != null && response.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                return View(product);
            }

            TempData["error"] = response?.Message ?? "Không tìm thấy sản phẩm.";
            return RedirectToAction("ViewAllProduct");
        }


        public async Task<IActionResult> OrderList() 
		{
			ResponseDTO? response = await _orderService.GetAllOrder();
			try
			{
				if (response != null && response.IsSuccess)
				{
					List<OrderDTO> orderList = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Result.ToString());
					return View(orderList);
				}
				else 
				{
					TempData["error"] = response.Message;
					return RedirectToAction("Index", "SaleStaff");
				}
			}
			catch (Exception ex) 
			{
				TempData["error"] = response.Message;
				return RedirectToAction("Index", "SaleStaff");
			}
		}

		[HttpGet]
		public async Task<IActionResult> OrderDetail(int id)
		{
			ResponseDTO? response = await _orderService.GetOrderByID(id);
			try
			{
				if (response != null && response.IsSuccess)
				{
					OrderDTO order = JsonConvert.DeserializeObject<OrderDTO>(response.Result.ToString());
					return View(order);
				}
				else
				{
					TempData["error"] = response.Message;
					return RedirectToAction("Index", "SaleStaff");
				}
			}
			catch (Exception ex)
			{
				TempData["error"] = response.Message;
				return RedirectToAction("Index", "SaleStaff");
			}
		}

		[HttpPost]
		public async Task<IActionResult> UpdateOrderStatus(OrderDTO order)
		{
			try
			{
				var response = await _orderService.UpdateOrderStatus(order.OrderId, order.Status);

                if (response != null && response.IsSuccess)
				{
					TempData["success"] = response.Message;
					return RedirectToAction("OrderList", "SaleStaff");
				}
				else
				{
					TempData["UpdateErrorMessage"] = response?.Message ?? "Cập nhật order status thất bại";
				}
			}
			catch (Exception ex)
			{
				TempData["UpdateErrorMessage"] = ex.Message;
			}
			return RedirectToAction("OrderDetail", new { id = order.OrderId });
		}
	}
}
