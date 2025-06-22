using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
    public class EventStaffController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IBlogFavoriteService _blogFavoriteService;
        private readonly IBlogReportService _blogReportService;
        private readonly IFeedbackService _feedbackService;
        private readonly IReportService _reportService;
        private readonly IVolunteerService _volunteerService;
        private readonly IUserService _userService;
        public EventStaffController(IBlogPostService blogPostService, IBlogFavoriteService blogFavoriteService, IBlogReportService blogReportService, IFeedbackService feedbackService, IReportService reportService, IVolunteerService volunteerService, IUserService userService)
        {
            this._blogPostService = blogPostService;
            this._blogFavoriteService = blogFavoriteService;
            _blogReportService = blogReportService;
            _feedbackService = feedbackService;
            _reportService = reportService;
            _volunteerService = volunteerService;
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewPendingPostDetail(int id)
        {
            BlogPostDTO blogPost = null;
            ResponseDTO? response = await _blogPostService.GetByBlogId(id);
            if (response != null && response.IsSuccess)
            {
                blogPost = JsonConvert.DeserializeObject<BlogPostDTO>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = "Không tìm thấy bài viết.";
                return RedirectToAction("ViewPendingPosts");
            }
            
            return View(blogPost);
        }

        //Pending Post
        public async Task<IActionResult> ViewPendingPosts()
        {
            List<BlogPostDTO> listProduct = new();
            ResponseDTO? response = await _blogPostService.GetAllPendingPost();
            if (response != null && response.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<BlogPostDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(listProduct);
        }

        [HttpPost]
        public async Task<IActionResult> BlogApprove(int id)
        {
            ResponseDTO? response = await _blogPostService.BlogApproval(id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Phê duyệt bài viết thành công.";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Phê duyệt thất bại.";
            }

            return RedirectToAction("ViewPendingPosts");
        }

        public async Task<IActionResult> ViewVolunteerRegistrations()
        {
            var response = await _volunteerService.GetAllVolunteerRegistrations();

            if (response != null && response.IsSuccess)
            {
                List<VolunteerDTO> volunteerList = JsonConvert.DeserializeObject<List<VolunteerDTO>>(response.Result.ToString());

                List<VolunteerWithUserViewModel> registrations = new();
                foreach (var volunteer in volunteerList)
                {
                    var userResponse = await _userService.GetUserById(volunteer.UserId);
                    if (userResponse != null && userResponse.IsSuccess)
                    {
                        UserDTO user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                        registrations.Add(new VolunteerWithUserViewModel
                        {
                            Volunteer = volunteer,
                            User = user
                        });
                    }
                    else
                    {
                        TempData["error"] = userResponse.Message;
                        return RedirectToAction("Index", "EventStaff");
                    }
                }

                var viewModel = new VolunteerRegistrationsViewModel
                {
                    Registrations = registrations
                };

                return View(viewModel);
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể lấy danh sách tình nguyện viên.";
                return View(new VolunteerRegistrationsViewModel { Registrations = new() });
            }
        }

        //public async Task<IActionResult> ViewVolunteerRegistrationDetail(int id)
        //{
        //    VolunteerDTO item = null;
        //    var response = await _volunteerService.GetVolunteerRegistrationById(id);

        //    if (response != null && response.IsSuccess)
        //    {
        //        item = JsonConvert.DeserializeObject<VolunteerDTO>(response.Result.ToString());
        //    }
        //    else
        //    {
        //        TempData["error"] = "Không tìm thấy thông tin tình nguyện viên.";
        //        return RedirectToAction("ViewVolunteerRegistrations");
        //    }

        //    return View(item);
        //}

        public async Task<IActionResult> ViewVolunteerRegistrationDetail(int id)
        {
            var response = await _volunteerService.GetVolunteerRegistrationById(id);

            if (response != null && response.IsSuccess)
            {
                var volunteer = JsonConvert.DeserializeObject<VolunteerDTO>(response.Result.ToString());

                var userResponse = await _userService.GetUserById(volunteer.UserId);
                if (userResponse != null && userResponse.IsSuccess)
                {
                    var user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());

                    var viewModel = new VolunteerWithUserViewModel
                    {
                        Volunteer = volunteer,
                        User = user
                    };

                    return View(viewModel);
                }
                else
                {
                    TempData["error"] = userResponse?.Message ?? "Không thể lấy thông tin người dùng.";
                    return RedirectToAction("ViewVolunteerRegistrations");
                }
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không tìm thấy thông tin tình nguyện viên.";
                return RedirectToAction("ViewVolunteerRegistrations");
            }
        }


        [HttpPost]
        public async Task<IActionResult> ApproveVolunteer(int id)
        {
            var response = await _volunteerService.ApproveVolunteerRegistration(id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Phê duyệt thành công.";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Phê duyệt thất bại.";
            }

            return RedirectToAction("ViewVolunteerRegistrations");
        }

        public async Task<IActionResult> ViewTeamLeaderRegistrations()
        {
            var response = await _volunteerService.GetAllTeamLeaderRegistrations();

            if (response != null && response.IsSuccess)
            {
                List<VolunteerDTO> volunteerList = JsonConvert.DeserializeObject<List<VolunteerDTO>>(response.Result.ToString());

                List<VolunteerWithUserViewModel> registrations = new();
                foreach (var volunteer in volunteerList)
                {
                    var userResponse = await _userService.GetUserById(volunteer.UserId);
                    if (userResponse != null && userResponse.IsSuccess)
                    {
                        UserDTO user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                        registrations.Add(new VolunteerWithUserViewModel
                        {
                            Volunteer = volunteer,
                            User = user
                        });
                    }
                    else
                    {
                        TempData["error"] = userResponse.Message;
                        return RedirectToAction("Index", "EventStaff");
                    }
                }

                var viewModel = new VolunteerRegistrationsViewModel
                {
                    Registrations = registrations
                };

                return View(viewModel);
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể lấy danh sách tình nguyện viên.";
                return View(new VolunteerRegistrationsViewModel { Registrations = new() });
            }
        }


        public async Task<IActionResult> ViewTeamLeaderRegistrationDetail(int id)
        {
            var response = await _volunteerService.GetTeamLeaderRegistrationById(id);

            if (response != null && response.IsSuccess)
            {
                var volunteer = JsonConvert.DeserializeObject<VolunteerDTO>(response.Result.ToString());

                var userResponse = await _userService.GetUserById(volunteer.UserId);
                if (userResponse != null && userResponse.IsSuccess)
                {
                    var user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());

                    var viewModel = new VolunteerWithUserViewModel
                    {
                        Volunteer = volunteer,
                        User = user
                    };

                    return View(viewModel);
                }
                else
                {
                    TempData["error"] = userResponse?.Message ?? "Không thể lấy thông tin người dùng.";
                    return RedirectToAction("ViewTeamLeaderRegistrations");
                }
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không tìm thấy thông tin tình nguyện viên.";
                return RedirectToAction("ViewTeamLeaderRegistrations");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ApproveTeamLeader(int id)
        {
            var response = await _volunteerService.ApproveTeamLeaderRegistration(id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Phê duyệt thành công.";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Phê duyệt thất bại.";
            }

            return RedirectToAction("ViewTeamLeaderRegistrations");
        }

    }
}
