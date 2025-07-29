using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Notification;
using GreenCorner.MVC.Models.VNPay;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        private readonly INotificationService _notificationService;
        private readonly IVnPayService _vnPayService;

        public OrderController(IOrderService orderService, ICartService cartService, IUserService userService, IVnPayService vnPayService, IAdminService adminService, INotificationService notificationService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userService = userService;
            _vnPayService = vnPayService;
            _adminService = adminService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Vui lòng đăng nhập để thanh toán";
                return RedirectToAction("Login", "Auth");
            }
            try
            {

                var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                var response = await _cartService.GetUserCart(userId);
                List<CartDTO> cartItems = new List<CartDTO>();
                if (response.Result != null && response.IsSuccess)
                {
                    cartItems = JsonConvert.DeserializeObject<List<CartDTO>>(Convert.ToString(response.Result));

                    var checkoutViewModel = new CheckoutViewModel
                    {
                        CartItems = cartItems,
                        Order = new OrderDTO()
                    };
                    return View(checkoutViewModel);
                }
                else
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            var orderDTO = model.Order;
            try
            {
                var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                var userName = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Name)?.FirstOrDefault()?.Value;
                var carts = await _cartService.GetUserCart(userId);
                List<CartDTO> cartItems = new List<CartDTO>();
                if (carts.Result != null && carts.IsSuccess)
                {
                    cartItems = JsonConvert.DeserializeObject<List<CartDTO>>(Convert.ToString(carts.Result));
                }
                var orderDetailDTOs = new List<OrderDetailDTO>();
                int total = 0;
                foreach (var item in cartItems)
                {
                    int price = (int)item.Product.Price;
                    int priceDiscount = price - (price * item.Product.Discount.Value / 100);
                    var totalPrice = (price - (price * item.Product.Discount.Value / 100)) * item.Quantity;
                    total += priceDiscount * item.Quantity;
                    orderDetailDTOs.Add(new OrderDetailDTO
                    {
                        OrderDetailId = 0,
                        OrderId = orderDTO.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = priceDiscount,
                        ProductDTO = item.Product
                    });
                }
                orderDTO.OrderDetailsDTO = orderDetailDTOs;
                orderDTO.TotalMoney = total;
                orderDTO.UserId = userId;
                orderDTO.CreatedAt = DateTime.Now;
                if (orderDTO.Note == null)
                {
                    orderDTO.Note = "";
                }

                if (orderDTO.PaymentMethod == "Chuyển khoản ngân hàng")
                {
                    TempData["OrderDTO"] = JsonConvert.SerializeObject(orderDTO);

                    PaymentInformationModel paymentInfo = new PaymentInformationModel
                    {
                        UserId = userId,
                        Amount = total,
                        OrderDescription = "Thanh toán đơn hàng",
                        OrderType = "order"
                    };

                    var paymentURL = _vnPayService.CreatePaymentUrl(paymentInfo, HttpContext);
                    return Redirect(paymentURL);
                }
                else
                {
                    orderDTO.Status = "Chờ xác nhận";
                    var response = await _orderService.CreateOrder(orderDTO);
                    if (response != null && response.IsSuccess)
                    {
                        foreach (var item in cartItems)
                        {
                            await _cartService.DeleteItemInCart(item.CartId);
                        }
                        return RedirectToAction("OrderCODComplete", "Order");
                    }
                    else
                    {
                        TempData["error"] = response.Message;
                        return RedirectToAction("OrderFail", "Order");

                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("OrderFail", "Order");
            }
        }

        [HttpGet]
        public async Task<IActionResult> OrderComplete()
        {
			var response = _vnPayService.PaymentExecute(Request.Query);
			if (response == null || response.VnPayResponseCode != "00")
			{
				TempData["error"] = "Thanh toán không thành công. Vui lòng thực hiện lại";
				return RedirectToAction("OrderFail", "Order");
			}

			try
			{
				var orderDTOJson = TempData["OrderDTO"] as string;
				if (string.IsNullOrEmpty(orderDTOJson))
				{
					TempData["error"] = "Không tìm thấy thông tin đơn hàng sau thanh toán";
					return RedirectToAction("OrderFail", "Order");
				}

				var orderDTO = JsonConvert.DeserializeObject<OrderDTO>(orderDTOJson);
				orderDTO.Status = "Đã thanh toán";
				orderDTO.CreatedAt = DateTime.Now;

				var result = await _orderService.CreateOrder(orderDTO);
				if (result != null && result.IsSuccess)
				{
					var deleteUserCart = await _cartService.DeleteUserCart(orderDTO.UserId);
					if (deleteUserCart != null && deleteUserCart.IsSuccess)
					{
						TempData["success"] = "Thanh toán thành công. Cảm ơn bạn đã mua hàng tại Green Corner";
					}
					else
					{
						TempData["error"] = deleteUserCart.Message;
						return RedirectToAction("OrderFail", "Order");
					}
				}
				else
				{
					TempData["error"] = result?.Message ?? "Lỗi tạo đơn hàng";
					return RedirectToAction("OrderFail", "Order");
				}

				return View();
			}
			catch (Exception ex)
			{
				TempData["error"] = ex.Message;
				return RedirectToAction("OrderFail", "Order");
			}
		}
        [HttpGet]
        public async Task<IActionResult> OrderCODComplete()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> OrderFail()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> History()
        {
            try
            {
                var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                if (userId == null)
                {
                    TempData["error"] = "Không tìm thấy lịch sử đơn hàng";
                    return RedirectToAction("Index", "Home");
                }
                var orders = await _orderService.GetOrderByUserID(userId);
                if (orders.IsSuccess)
                {
                    var orderList = JsonConvert.DeserializeObject<List<OrderDTO>>(Convert.ToString(orders.Result));
                    return View(orderList);
                }
                else
                {
                    TempData["error"] = orders.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

		[HttpPost]
		public async Task<IActionResult> CancelOrder(int orderId)
        {
            try
            {
                var response = await _orderService.UpdateOrderStatus(orderId, "Hủy đơn hàng");
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hủy đơn hàng thành công";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction("History", "Order");

            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = ex.Message });
            }
        }

        public async Task<IActionResult> ListOrder()
        {
            List<OrderDTO> orders = new();
            ResponseDTO? response = await _orderService.GetAllOrder();

            if (response != null && response.IsSuccess)
            {
                orders = JsonConvert.DeserializeObject<List<OrderDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response.Message ?? "Error";
                return RedirectToAction("Index", "Admin");
            }

            return View(orders);
        }


        [HttpGet]
        public async Task<IActionResult> OrderDetail(int id)
        {
            ResponseDTO? response = await _orderService.GetOrderByID(id);
            OrderDTO order = new();
            if (response != null && response.IsSuccess)
            {
                order = JsonConvert.DeserializeObject<OrderDTO>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response.Message ?? "Error";
                return RedirectToAction("ListOrders", "Admin");
            }

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(OrderDTO order)
        {
            try
            {
                var response = await _orderService.UpdateOrderStatus(order.OrderId, order.Status);
                if (response != null && response.IsSuccess)
                {
                    var notification = new NotificationDTO
                    {
                        UserId = order.UserId,
                        Title = "Đơn hàng đã được cập nhật",
                        Message = $"Đơn hàng của bạn đã được cập nhật trạng thái thành '{order.Status}'."
                    };
                    var sendNotification = await _notificationService.SendNotification(notification);
                    if (sendNotification != null && sendNotification.IsSuccess)
                    {
                        TempData["success"] = "Đã cập nhật tình trạng đơn hàng và gửi thông báo cho người dùng!";
                        var StaffName = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Name).FirstOrDefault()?.Value;
                        var log = new SystemLogDTO()
                        {
                            UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value,
                            ActionType = "Cập nhật trạng thái",
                            ObjectType = "Đơn hàng",
                            ObjectId = order.OrderId,
                            Description = $"Nhân viên {StaffName} đã cập nhật trạng thái đơn hàng {order.OrderId} thành {order.Status}",
                            CreatedAt = DateTime.Now,
                        };
                        var logResponse = await _adminService.AddLogStaff(log);
                    }
                    else
                    {
                        TempData["error"] = sendNotification?.Message ?? "Đã có lỗi xảy ra.";
                    }
                }
                else
                {
                    TempData["error"] = response?.Message ?? "Lỗi cập nhật trạng thái đơn hàng";
                }
                return RedirectToAction(nameof(ListOrder));
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction(nameof(ListOrder));
            }
        }
    }
}