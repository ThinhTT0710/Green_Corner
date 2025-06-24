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
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { isSuccess = false, message = "Please login to add item to cart" });
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


        [HttpGet]
        public async Task<IActionResult> AddToCart(int productId)
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Json(new { isSuccess = false, message = "Please login to add item to cart" });
                }
                var userID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                var cartDto = new CartDTO
                {
                    UserId = userID,
                    ProductId = productId,
                    Quantity = 1
                };
                var response = await _cartService.AddToCart(cartDto);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Added to cart!";
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = ex.Message });
            }
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCart(CartDTO cartDTO)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Json(new { isSuccess = false, message = "Please login to update item to cart" });
            }
            try
            {
                cartDTO.UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                var response = await _cartService.UpdateCart(cartDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Item Updated from cart";
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

        [HttpGet]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> Remove(int cartId)
        {
            try
            {
                var response = await _cartService.DeleteItemInCart(cartId);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Item removed from cart";
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
