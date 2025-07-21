using GreenCorner.MVC.Models.Chat;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GreenCorner.MVC.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatService _chatService;
        private readonly IVolunteerService _volunteerService;
        public ChatController(IChatService chatService, IVolunteerService volunteerService)
        {
            _chatService = chatService;
            _volunteerService = volunteerService;
        }

        public async Task<IActionResult> Index(int eventId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Vui lòng đăng nhập để có thể nhắn tin trong sự kiện";
                return RedirectToAction("Error404", "Home");
            }
            try
            {
                if (eventId <= 0)
                {
                    TempData["error"] = "Sự kiện không hợp lệ";
                    return RedirectToAction("Index", "Home");
                }
                var userID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
                var checkConfirmVolunteer = await _volunteerService.IsConfirmVolunteer(eventId, userID);
                if (checkConfirmVolunteer != null && checkConfirmVolunteer.IsSuccess)
                {
                    var response = await _chatService.GetChatMessagesAsync(eventId);
                    if (response != null || response.IsSuccess)
                    {
                        var chatHistory = JsonConvert.DeserializeObject<List<ChatMessageDTO>>(Convert.ToString(response.Result));
                        ViewBag.EventId = eventId;
                        ViewBag.SenderId = userID;
                        ViewBag.SenderName = User.FindFirst(ClaimTypes.Name)?.Value;
                        ViewBag.SenderAvatar = User.FindFirst("avatar")?.Value ?? "/imgs/avatars/default.png";

                        return View(chatHistory);
                    }
                    else
                    {
                        TempData["error"] = response?.Message;
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                    {
                        TempData["error"] = "Bạn chưa đăng ký tham gia sự kiện này hoặc chưa được phê duyệt làm tình nguyện viên.";
                        return RedirectToAction("Index", "Home");
                    }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Đã xảy ra lỗi: " + ex.Message;
                return RedirectToAction("Error404", "Home");
            }
        }
    }
}
