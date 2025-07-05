using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class WishListController : Controller
    {
        private readonly IWishListService _wishListService;

        public WishListController(IWishListService wishListService)
        {
            _wishListService = wishListService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Vui lòng đăng nhập để xem danh sách yêu thích của bạn";
                return Redirect(Request.Headers["Referer"].ToString());
            }
            try
            {
                var userID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                var response = await _wishListService.GetUserWishList(userID);
                if (response != null && response.IsSuccess)
                {
                    List<WishListDTO> wishLists = JsonConvert.DeserializeObject<List<WishListDTO>>(Convert.ToString(response.Result));
                    return View(wishLists);
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

        [HttpGet]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<IActionResult> Remove(int wishListId)
        {
            try
            {
                var response = await _wishListService.Delete(wishListId);
                if (response != null && response.IsSuccess)
                {
                    return Json(new { isSuccess = true, message = "Sản phẩm đã được xóa khỏi danh sách yêu thích." });
                }
                else
                {
                    return Json(new { isSuccess = false, message = response?.Message ?? "Không thể xóa sản phẩm khỏi danh sách yêu thích." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { isSuccess = false, message = $"Đã xảy ra lỗi: {ex.Message}" });
            }
        }
    }
}
