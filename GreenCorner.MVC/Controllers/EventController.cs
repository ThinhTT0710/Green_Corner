using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;
        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }
        public async Task<IActionResult> Index()
        {
            List<EventDTO> listEvent = new();
            ResponseDTO? response = await _eventService.GetAllEvent();
            if (response != null && response.IsSuccess)
            {
                listEvent = JsonConvert.DeserializeObject<List<EventDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listEvent);
        }
        public async Task<IActionResult> RateEvent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RateEvent(EventReviewDTO eventReviewDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _eventService.RateEvent(eventReviewDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Event Rate successfully!";
                    return RedirectToAction(nameof(EventReviewHistory));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(eventReviewDTO);
        }
        public async Task<IActionResult> EventReviewHistory(string id)
        {
            List<EventReviewDTO> listEventReview = new();
            ResponseDTO? response = await _eventService.ViewEventReviewHistory("2");
            if (response != null && response.IsSuccess)
            {
                listEventReview = JsonConvert.DeserializeObject<List<EventReviewDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listEventReview);
        }

        public async Task<IActionResult> DeleteEventReview(int eventReviewId)
        {
            ResponseDTO response = await _eventService.GetEventReviewById(eventReviewId);
            if (response != null && response.IsSuccess)
            {
                EventReviewDTO eventReviewDTO = JsonConvert.DeserializeObject<EventReviewDTO>(response.Result.ToString());
                return View(eventReviewDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEventReview(EventReviewDTO eventReviewDTO)
        {
            ResponseDTO response = await _eventService.DeleteEventReview(eventReviewDTO.EventReviewId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Event review deleted successfully!";
                return RedirectToAction(nameof(EventReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(eventReviewDTO);
        }

        public async Task<IActionResult> EditEventReview(int eventReviewId)
        {
            ResponseDTO response = await _eventService.GetEventReviewById(eventReviewId);
            if (response != null && response.IsSuccess)
            {
                EventReviewDTO eventReview = JsonConvert.DeserializeObject<EventReviewDTO>(response.Result.ToString());
                return View(eventReview);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditEventReview(EventReviewDTO eventReviewDTO)
        {
            ResponseDTO response = await _eventService.EditEventReview(eventReviewDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Event review updated successfully!";
                return RedirectToAction(nameof(EventReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(eventReviewDTO);
        }
        public async Task<IActionResult> LeaderReview()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _eventService.LeaderReview(leaderReviewDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Leader review successfully!";
                    return RedirectToAction(nameof(LeaderReviewHistory));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(leaderReviewDTO);
        }
        public async Task<IActionResult> LeaderReviewHistory(string id)
        {
            List<LeaderReviewDTO> listLeaderReview = new();
            ResponseDTO? response = await _eventService.ViewLeaderReviewHistory("2");
            if (response != null && response.IsSuccess)
            {
                listLeaderReview = JsonConvert.DeserializeObject<List<LeaderReviewDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listLeaderReview);
        }

        public async Task<IActionResult> DeleteLeaderReview(int leaderReviewId)
        {
            ResponseDTO response = await _eventService.GetLeaderReviewById(leaderReviewId);
            if (response != null && response.IsSuccess)
            {
                LeaderReviewDTO leaderReviewDTO = JsonConvert.DeserializeObject<LeaderReviewDTO>(response.Result.ToString());
                return View(leaderReviewDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteLeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            ResponseDTO response = await _eventService.DeleteLeaderReview(leaderReviewDTO.LeaderReviewId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Leader review deleted successfully!";
                return RedirectToAction(nameof(LeaderReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(leaderReviewDTO);
        }

        public async Task<IActionResult> EditLeaderReview(int leaderReviewId)
        {
            ResponseDTO response = await _eventService.GetLeaderReviewById(leaderReviewId);
            if (response != null && response.IsSuccess)
            {
                LeaderReviewDTO leaderReviewDTO = JsonConvert.DeserializeObject<LeaderReviewDTO>(response.Result.ToString());
                return View(leaderReviewDTO);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditLeaderReview(LeaderReviewDTO leaderReviewDTO)
        {
            ResponseDTO response = await _eventService.EditLeaderReview(leaderReviewDTO);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Leader review updated successfully!";
                return RedirectToAction(nameof(LeaderReviewHistory));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(leaderReviewDTO);
        }
    }
}
