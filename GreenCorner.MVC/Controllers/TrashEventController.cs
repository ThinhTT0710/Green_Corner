using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class TrashEventController : Controller
    {
        private readonly ITrashEventService _trashEventService;

        public TrashEventController(ITrashEventService trashEventService)
        {
            _trashEventService = trashEventService;
        }

        public async Task<IActionResult> Index()
        {
            List<TrashEventDTO> listTrashEvents = new();
            ResponseDTO? response = await _trashEventService.GetAllTrashEvent();
            if (response != null && response.IsSuccess)
            {
                listTrashEvents = JsonConvert.DeserializeObject<List<TrashEventDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listTrashEvents);
        }


        public async Task<IActionResult> ReportTrashEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReportTrashEvent(TrashEventDTO trashEventDTO)
        {
            if (!ModelState.IsValid)
            {
                trashEventDTO.UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
                trashEventDTO.CreatedAt = DateTime.Now;
                trashEventDTO.Status = "Chờ xác nhận";
                ResponseDTO response = await _trashEventService.AddTrashEvent(trashEventDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Trash event reported successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(trashEventDTO);
        }


        public async Task<IActionResult> DeleteTrashEvent(int trashReportId)
        {
            ResponseDTO response = await _trashEventService.GetByTrashEventId(trashReportId);
            if (response != null && response.IsSuccess)
            {
                TrashEventDTO trashEventDTO = JsonConvert.DeserializeObject<TrashEventDTO>(response.Result.ToString());
                return View(trashEventDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTrashEvent(TrashEventDTO trashEventDTO)
        {
            ResponseDTO response = await _trashEventService.DeleteTrashEvent(trashEventDTO.TrashReportId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Trash Event deleted successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(trashEventDTO);
        }

        public async Task<IActionResult> UpdateTrashEvent(int trashReportId)
        {
            ResponseDTO response = await _trashEventService.GetByTrashEventId(trashReportId);
            if (response != null && response.IsSuccess)
            {
                TrashEventDTO trashEventDTO= JsonConvert.DeserializeObject<TrashEventDTO>(response.Result.ToString());
                return View(trashEventDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrashEvent(TrashEventDTO trashEventDTO)
        {
			trashEventDTO.UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value;
			ResponseDTO response = await _trashEventService.UpdateTrashEvent(trashEventDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Trash event updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(trashEventDTO);
        }

        [HttpGet]
        public async Task<IActionResult> ApproveTrashEvent(int trashReportId)
        {
            ResponseDTO response = await _trashEventService.ApproveTrashEvent(trashReportId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = response?.Message;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return RedirectToAction(nameof(Index));
        }

		[HttpGet]
		public async Task<IActionResult> RejectrashEvent(int trashReportId)
		{
			ResponseDTO response = await _trashEventService.RejectTrashEvent(trashReportId);
			if (response != null && response.IsSuccess)
			{
				TempData["success"] = response?.Message;
				return RedirectToAction(nameof(Index));
			}
			else
			{
				TempData["error"] = response?.Message;
			}
			return RedirectToAction(nameof(Index));
		}
	}
}
