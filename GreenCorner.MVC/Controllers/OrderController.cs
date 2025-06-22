using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Momo;
using GreenCorner.MVC.Models.VNPay;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
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
        private readonly IVnPayService _vnPayService;
        private readonly IMomoService _momoService;

        public OrderController(IOrderService orderService, ICartService cartService, IUserService userService, IVnPayService vnPayService, IMomoService momoService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _userService = userService;
            _vnPayService = vnPayService;
            _momoService = momoService;
        }

        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Please login to checkout";
                return Redirect(Request.Headers["Referer"].ToString());
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

                if (orderDTO.PaymentMethod == "Thanh toán bằng VNPay")
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
                else if (orderDTO.PaymentMethod == "Thanh toán bằng Momo")
                {
                    TempData["OrderDTO"] = JsonConvert.SerializeObject(orderDTO);

                    OrderInfoModel paymentInfo = new OrderInfoModel
                    {
                        FullName = userName,
                        OrderId = Guid.NewGuid().ToString(),
                        OrderInfo = "Thanh toán đơn hàng tại GreenCorner",
                        Amount =(double) total,
                    };

                    var response = await _momoService.CreatePaymentAsync(paymentInfo);
                    return Redirect(response.PayUrl);
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
                        if (orderDTO.PaymentMethod == "Chuyển khoản ngân hàng")
                        {

                        }
                        return RedirectToAction("OrderComplete", "Order");
                    }
                    else
                    {
                        TempData["error"] = response.Message;
                        return View(new CheckoutViewModel
                        {
                            Order = orderDTO,
                            CartItems = cartItems
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View(new CheckoutViewModel
                {
                    Order = orderDTO,
                    CartItems = new List<CartDTO>()
                });
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
        public async Task<IActionResult> History()
        {
            try
            {
                var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                if (userId == null)
                {
                    TempData["error"] = "User not found";
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
                var response = await _orderService.DeleteOrder(orderId);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Hủy đơn hàng thành công";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = ex.Message });
            }
        }
    }
}