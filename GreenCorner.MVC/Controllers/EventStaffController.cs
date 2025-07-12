using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

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
        private readonly IPointTransactionService _pointTransactionService;
        public EventStaffController(IBlogPostService blogPostService, IBlogFavoriteService blogFavoriteService, IBlogReportService blogReportService, IFeedbackService feedbackService, IReportService reportService, IVolunteerService volunteerService, IUserService userService, IPointTransactionService pointTransactionService)
        {
            this._blogPostService = blogPostService;
            this._blogFavoriteService = blogFavoriteService;
            _blogReportService = blogReportService;
            _feedbackService = feedbackService;
            _reportService = reportService;
            _volunteerService = volunteerService;
            _userService = userService;
            _pointTransactionService = pointTransactionService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ViewPendingPostDetail(int id)
        {
            BlogPostDTO blogPost = null;
            UserDTO author = null;

            // Lấy bài viết
            ResponseDTO? response = await _blogPostService.GetByBlogId(id);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                blogPost = JsonConvert.DeserializeObject<BlogPostDTO>(response.Result.ToString());

                // Lấy tác giả
                var userResponse = await _userService.GetUserById(blogPost.AuthorId);
                if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                {
                    author = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                }
            }
            else
            {
                TempData["error"] = "Không tìm thấy bài viết.";
                return RedirectToAction("ViewPendingPosts");
            }

            // Gửi viewmodel sang view
            var viewModel = new BlogWithAuthorViewModel
            {
                Blog = blogPost,
                Author = author
            };

            return View(viewModel);
        }


        //Pending Post
        public async Task<IActionResult> ViewPendingPosts()
        {
            List<BlogWithAuthorViewModel> blogsWithAuthors = new();

            ResponseDTO? response = await _blogPostService.GetAllPendingPost();
            if (response != null && response.IsSuccess && response.Result != null)
            {
                var listProduct = JsonConvert.DeserializeObject<List<BlogPostDTO>>(response.Result.ToString());

                foreach (var blog in listProduct)
                {
                    UserDTO? author = null;

                    var userResponse = await _userService.GetUserById(blog.AuthorId);
                    if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                    {
                        author = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                    }

                    blogsWithAuthors.Add(new BlogWithAuthorViewModel
                    {
                        Blog = blog,
                        Author = author
                    });
                }
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(blogsWithAuthors);
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

        [HttpPost]
        public async Task<IActionResult> RejectVolunteer(int id)
        {
            var response = await _volunteerService.RejectVolunteerRegistration(id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Từ chối thành công.";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Từ chối thất bại.";
            }

            return RedirectToAction("ViewVolunteerRegistrations");
        }

        [HttpPost]
        public async Task<IActionResult> RejectTeamLeader(int id)
        {
            var response = await _volunteerService.RejectTeamLeaderRegistration(id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Từ chối thành công.";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Từ chối thất bại.";
            }

            return RedirectToAction("ViewTeamLeaderRegistrations");
        }

        public async Task<IActionResult> ViewUsersWithParticipation()
        {
            var response = await _volunteerService.GetUserWithParticipation();

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response?.Message ?? "Lỗi khi lấy danh sách người dùng đã tham gia.";
                return RedirectToAction("Index", "Home");
            }

            // Parse danh sách userId từ response.Result
            var userIds = JsonConvert.DeserializeObject<List<string>>(response.Result.ToString());

            if (userIds == null || !userIds.Any())
            {
                TempData["error"] = "Không có người dùng nào tham gia hoạt động.";
                return RedirectToAction("Index", "Home");
            }

            List<VolunteerWithUserViewModel> result = new();

            foreach (var userId in userIds)
            {
                var userResponse = await _userService.GetUserById(userId);
                if (userResponse != null && userResponse.IsSuccess)
                {
                    var user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());

                    result.Add(new VolunteerWithUserViewModel
                    {
                        User = user
                    });
                }
                else
                {
                    TempData["error"] = userResponse?.Message ?? $"Không thể lấy thông tin người dùng có ID: {userId}";
                    return RedirectToAction("Index", "Home");
                }
            }

            var finalViewModel = new VolunteerRegistrationsViewModel
            {
                Registrations = result
            };

            return View(finalViewModel);
        }

        public async Task<IActionResult> ViewPointsRewardHistory()
        {
            var response = await _pointTransactionService.GetPointsAwardHistoryAsync();

            if (response != null && response.IsSuccess && response.Result != null)
            {
                var transactions = JsonConvert.DeserializeObject<List<PointTransactionDTO>>(response.Result.ToString());
                var resultList = new List<PointTransactionWithUserDTO>();

                foreach (var transaction in transactions)
                {
                    var userResponse = await _userService.GetUserById(transaction.UserId); // Gọi đến API người dùng
                    UserDTO? user = null;

                    if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                    {
                        user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                    }

                    resultList.Add(new PointTransactionWithUserDTO
                    {
                        Transaction = transaction,
                        User = user ?? new UserDTO { FullName = "Không xác định" }
                    });
                }

                return View(resultList);
            }

            TempData["error"] = "Không thể tải lịch sử.";
            return View(new List<PointTransactionWithUserDTO>());
        }




    }
}
