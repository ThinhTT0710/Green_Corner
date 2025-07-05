using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Vui lòng đăng nhập để xem giỏ hàng của bạn";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            try
            {
            var userID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _cartService.GetUserCart(userID);
                if (response != null && response.IsSuccess)
                {
                    List<CartDTO> cart = JsonConvert.DeserializeObject<List<CartDTO>>(Convert.ToString(response.Result));
                    return View(cart);
                }
                else
                {
                    TempData["error"] = response.Message;
                    return RedirectToAction("Index", "Home");
                }
            }
            catch(Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
            return View(new List<CartDTO>());
        }


        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { isSuccess = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng." });
                }

                var userID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

                if (string.IsNullOrEmpty(userID))
                {
                    return Json(new { isSuccess = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại." });
                }

                var cartDto = new CartDTO
                {
                    UserId = userID,
                    ProductId = productId,
                    Quantity = 1
                };

                var response = await _cartService.AddToCart(cartDto);

                if (response != null && response.IsSuccess)
                {
                    return Json(new { isSuccess = true, message = "Sản phẩm đã được thêm vào giỏ hàng!", icon = "success" });
                }
                else
                {
                    return Json(new { isSuccess = false, message = response?.Message ?? "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.", icon = "error" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = $"Đã xảy ra lỗi hệ thống: {ex.Message}", icon = "error" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToCartWithQuantity(int productId, int quantity)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { isSuccess = false, message = "Vui lòng đăng nhập để thêm sản phẩm vào giỏ hàng.", icon = "error" });
                }

                if (quantity < 1)
                {
                    return Json(new { isSuccess = false, message = "Số lượng sản phẩm phải lớn hơn hoặc bằng 1.", icon = "warning" });
                }

                var userID = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(userID))
                {
                    return Json(new { isSuccess = false, message = "Không thể xác định người dùng. Vui lòng đăng nhập lại.", icon = "error" });
                }

                var cartDto = new CartDTO
                {
                    UserId = userID,
                    ProductId = productId,
                    Quantity = quantity
                };

                var response = await _cartService.AddToCart(cartDto); 

                if (response != null && response.IsSuccess)
                {
                    return Json(new { isSuccess = true, message = "Sản phẩm đã được thêm vào giỏ hàng!", icon = "success"});
                }
                else
                {
                    return Json(new { isSuccess = false, message = response?.Message ?? "Có lỗi xảy ra khi thêm sản phẩm vào giỏ hàng.", icon = "error" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = $"Đã xảy ra lỗi hệ thống: {ex.Message}", icon = "error" });
            }
        }

        public async Task<IActionResult> UpdateQuantity(int cartId, int productId, int quantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { isSuccess = false, message = "Vui lòng đăng nhập để cập nhật giỏ hàng." });
            }

            if (quantity < 1)
            {
                return Json(new { isSuccess = false, message = "Số lượng sản phẩm phải lớn hơn hoặc bằng 1." });
            }

            try
            {
                var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Json(new { isSuccess = false, message = "Không thể xác định người dùng." });
                }

                var cartDTO = new CartDTO
                {
                    CartId = cartId,
                    ProductId = productId, 
                    Quantity = quantity,
                    UserId = userId 
                };

                var response = await _cartService.UpdateCart(cartDTO);

                if (response != null && response.IsSuccess)
                {
                    return Json(new { isSuccess = true, message = "Số lượng sản phẩm đã được cập nhật thành công." });
                }
                else
                {
                    return Json(new { isSuccess = false, message = response?.Message ?? "Không thể cập nhật số lượng sản phẩm." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }

        [HttpGet] 
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> Remove(int cartId)
        {
            try
            {
                var response = await _cartService.DeleteItemInCart(cartId);
                if (response != null && response.IsSuccess)
                {
                    return Json(new { isSuccess = true, message = "Sản phẩm đã được xóa khỏi giỏ hàng." });
                }
                else
                {
                    return Json(new { isSuccess = false, message = response?.Message ?? "Không thể xóa sản phẩm khỏi giỏ hàng." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
    }
}
