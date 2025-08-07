using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace GreenCorner.MVC.Controllersde
{
    public class BlogPostController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IBlogFavoriteService _blogFavoriteService;
        private readonly IBlogReportService _blogReportService;
        private readonly IFeedbackService _feedbackService;
        private readonly IReportService _reportService;
        private readonly IUserService _userService;
        public BlogPostController(IBlogPostService blogPostService, IBlogFavoriteService blogFavoriteService, IBlogReportService blogReportService, IFeedbackService feedbackService, IReportService reportService, IUserService userService)
        {
            this._blogPostService = blogPostService;
            this._blogFavoriteService = blogFavoriteService;
            _blogReportService = blogReportService;
            _feedbackService = feedbackService;
            _reportService = reportService;
            _userService = userService;
        }

        //BlogPost

        public async Task<IActionResult> Index()
        {
            List<BlogWithAuthorViewModel> blogsWithAuthors = new();
            List<int> favoriteBlogIds = new();

            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            // Lấy tất cả blog
            ResponseDTO? blogResponse = await _blogPostService.GetAllBlogPost();

            if (blogResponse != null && blogResponse.IsSuccess && blogResponse.Result != null)
            {
                var blogList = JsonConvert.DeserializeObject<List<BlogPostDTO>>(blogResponse.Result.ToString());

                // Lấy danh sách blog yêu thích của người dùng
                if (!string.IsNullOrEmpty(userId))
                {
                    var favResponse = await _blogFavoriteService.GetFavoritesByUserAsync(userId);
                    if (favResponse != null && favResponse.IsSuccess && favResponse.Result != null)
                    {
                        var favorites = JsonConvert.DeserializeObject<List<BlogFavoriteDTO>>(favResponse.Result.ToString());
                        favoriteBlogIds = favorites.Select(f => f.BlogId).ToList();
                    }
                }

                foreach (var blog in blogList)
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

            // Gán danh sách BlogId đã yêu thích vào ViewBag để View biết mà hiển thị tim đỏ
            ViewBag.FavoriteBlogIds = favoriteBlogIds;

            return View(blogsWithAuthors);
        }


        public async Task<IActionResult> MyFavoriteBlogs()
        {
            List<BlogWithAuthorViewModel> blogsWithAuthors = new();
            List<int> favoriteBlogIds = new();

            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Không thể xác định người dùng. Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Auth");
            }

            // Lấy tất cả bài viết
            ResponseDTO? blogResponse = await _blogPostService.GetAllBlogPost();
            List<BlogPostDTO> listBlog = new();

            if (blogResponse != null && blogResponse.IsSuccess && blogResponse.Result != null)
            {
                listBlog = JsonConvert.DeserializeObject<List<BlogPostDTO>>(blogResponse.Result.ToString());
            }

            // Lấy danh sách blog yêu thích
            if (!string.IsNullOrEmpty(userId))
            {
                var favResponse = await _blogFavoriteService.GetFavoritesByUserAsync(userId);
                if (favResponse != null && favResponse.IsSuccess && favResponse.Result != null)
                {
                    var favorites = JsonConvert.DeserializeObject<List<BlogFavoriteDTO>>(favResponse.Result.ToString());
                    favoriteBlogIds = favorites.Select(f => f.BlogId).ToList();
                }
            }

            var favoriteBlogs = listBlog.Where(b => favoriteBlogIds.Contains(b.BlogId)).ToList();

            foreach (var blog in favoriteBlogs)
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

            ViewBag.FavoriteBlogIds = favoriteBlogIds;

            return View(blogsWithAuthors); // Trả về view dùng chung
        }



        public async Task<IActionResult> ViewBlogDetail(int id)
        {
            BlogWithAuthorViewModel viewModel = new();

            ResponseDTO? response = await _blogPostService.GetByBlogId(id);
            if (response != null && response.IsSuccess && response.Result != null)
            {
                var blogPost = JsonConvert.DeserializeObject<BlogPostDTO>(response.Result.ToString());
                viewModel.Blog = blogPost;

                var authorResponse = await _userService.GetUserById(blogPost.AuthorId);
                if (authorResponse != null && authorResponse.IsSuccess && authorResponse.Result != null)
                {
                    var author = JsonConvert.DeserializeObject<UserDTO>(authorResponse.Result.ToString());
                    viewModel.Author = author;
                }

                var reportResponse = await _blogReportService.GetReportsByBlogIdAsync(blogPost.BlogId);
                if (reportResponse != null && reportResponse.IsSuccess && reportResponse.Result != null)
                {
                    var reports = JsonConvert.DeserializeObject<List<BlogReportDTO>>(reportResponse.Result.ToString());
                    viewModel.Reports = reports ?? new();

                    var userIds = viewModel.Reports
                .Where(r => !string.IsNullOrEmpty(r.UserId))
                .Select(r => r.UserId!)
                .Distinct()
                .ToList();

                    var reportAuthors = new Dictionary<string, UserDTO>();

                    foreach (var reportUserId in userIds)
                    {
                        var userResponse = await _userService.GetUserById(reportUserId);
                        if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                        {
                            var user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                            if (user != null)
                                reportAuthors[reportUserId] = user;
                        }
                    }

                    ViewBag.ReportAuthors = reportAuthors;
                }

                var userId = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
                if (!string.IsNullOrEmpty(userId))
                {
                    var favResponse = await _blogFavoriteService.IsFavoritedAsync(blogPost.BlogId, userId);
                    ViewBag.IsFavorited = favResponse?.IsSuccess == true && (bool)favResponse.Result;
                }
                else
                {
                    ViewBag.IsFavorited = false;
                }
                ViewBag.UserId = userId;
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không tìm thấy bài viết.";
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }


        public async Task<IActionResult> CreateBlog()
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Auth");
            }

            var model = new BlogPostDTO
            {
                AuthorId = userId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogPostDTO blogDTO)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                blogDTO.AuthorId = userId;
            }

            var files = Request.Form.Files;
            var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                files, folderName: "blog", filePrefix: "blog");

            if (!isSuccess)
            {
                ModelState.AddModelError("Image", errorMessage);
                return View(blogDTO);
            }

            blogDTO.ThumbnailUrl = string.Join("&", imagePaths);

            if (ModelState.IsValid)
            {
                ResponseDTO response = await _blogPostService.AddBlog(blogDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Tạo Blog thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message ?? "Đã xảy ra lỗi khi tạo blog.";
                }
            }

            return View(blogDTO);
        }



        public async Task<IActionResult> UpdateBlog(int blogId)
        {
            ResponseDTO response = await _blogPostService.GetByBlogId(blogId);
            if (response != null && response.IsSuccess)
            {
                BlogPostDTO blog = JsonConvert.DeserializeObject<BlogPostDTO>(response.Result.ToString());
                return View(blog);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlog(BlogPostDTO blogDto)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["loginError"] = "Bạn cần đăng nhập để thực hiện chức năng này";
                return RedirectToAction("Login", "Auth");
            }

            var files = Request.Form.Files;
            bool hasNewImages = files != null && files.Count > 0;

            if (hasNewImages)
            {
                if (!string.IsNullOrEmpty(blogDto.ThumbnailUrl))
                {
                    foreach (var oldPath in blogDto.ThumbnailUrl.Split("&"))
                    {
                        var fullOldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldPath.TrimStart('/'));
                        if (System.IO.File.Exists(fullOldPath))
                        {
                            System.IO.File.Delete(fullOldPath);
                        }
                    }
                }

                var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                    files, folderName: "blog", filePrefix: "blog");

                if (!isSuccess)
                {
                    ModelState.AddModelError("Image", errorMessage);
                    return View(blogDto);
                }

                blogDto.ThumbnailUrl = string.Join("&", imagePaths);
            }

            ResponseDTO response = await _blogPostService.UpdateBlog(blogDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật Blog thành công!";
                return RedirectToAction(nameof(MyPendingBlogs));
            }
            else
            {
                TempData["error"] = response?.Message ?? "Đã xảy ra lỗi khi cập nhật Blog.";
            }

            return View(blogDto);
        }


        public async Task<IActionResult> DeleteBlog(int id)
        {
            ResponseDTO response = await _blogPostService.GetByBlogId(id);
            if (response != null && response.IsSuccess)
            {
                BlogPostDTO product = JsonConvert.DeserializeObject<BlogPostDTO>(response.Result.ToString());
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlog(BlogPostDTO blogDto)
        {
            ResponseDTO response = await _blogPostService.DeleteBlog(blogDto.BlogId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Xóa Blog thành công!";
                return RedirectToAction(nameof(MyPendingBlogs));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(blogDto);
        }

        //BlogFavorite
        [HttpPost]
        public async Task<IActionResult> AddToFavorite(int blogId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để thêm vào yêu thích.";
                return RedirectToAction(nameof(Index));
            }

            var dto = new BlogFavoriteAddDTO { BlogId = blogId, UserId = userId };
            ResponseDTO response = await _blogFavoriteService.AddFavoriteAsync(dto);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Đã thêm bài viết vào danh sách yêu thích!";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể thêm vào yêu thích.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromFavorite(int blogId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            ;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Bạn cần đăng nhập để xóa khỏi yêu thích.";
                return RedirectToAction(nameof(Index));
            }

            ResponseDTO response = await _blogFavoriteService.RemoveFavoriteAsync(blogId, userId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Đã xóa bài viết khỏi danh sách yêu thích!";
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể xóa khỏi yêu thích.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> IsFavorited(int blogId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Json(new ResponseDTO { IsSuccess = true, Result = false }); // chưa đăng nhập => chưa yêu thích
            }

            ResponseDTO response = await _blogFavoriteService.IsFavoritedAsync(blogId, userId);
            return Json(response ?? new ResponseDTO { IsSuccess = false, Message = "Không thể kiểm tra trạng thái yêu thích." });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleFavorite(int blogId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;


            if (string.IsNullOrEmpty(userId))
            {
                return Json(new ResponseDTO { IsSuccess = false, Message = "Bạn cần đăng nhập." });
            }

            var response = await _blogFavoriteService.ToggleFavoriteAsync(blogId, userId);
            return Json(response);
        }

        //BlogReport
        public async Task<IActionResult> CreateReport(int blogId)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Auth");
            }
            var model = new BlogReportDTO { BlogId = blogId, UserId = userId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromForm] BlogReportDTO reportDTO)
        {
            var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Vui lòng đăng nhập.";
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.First().ErrorMessage
                    );

                return Json(new { isSuccess = false, errors });
            }

            var response = await _blogReportService.CreateReportAsync(reportDTO);

            if (response != null && response.IsSuccess)
            {
                return Json(new { isSuccess = true });
            }

            return Json(new
            {
                isSuccess = false,
                message = response?.Message ?? "Có lỗi xảy ra khi gửi báo cáo."
            });
        }

        [HttpGet]
        public async Task<IActionResult> EditReport(int id)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDTO? response = await _blogReportService.GetReportById(id);

            if (response == null || !response.IsSuccess)
            {
                TempData["error"] = response?.Message ?? "Không thể lấy thông tin báo cáo.";
                return RedirectToAction("Index", "BlogPost");
            }

            var report = JsonConvert.DeserializeObject<BlogReportDTO>(Convert.ToString(response.Result)!);

            return View(report);
        }


        [HttpPost]
        public async Task<IActionResult> EditReport(BlogReportDTO model)
        {
            if (string.IsNullOrWhiteSpace(model.Reason))
            {
                return Json(new { isSuccess = false, errors = new { Reason = "Lý do báo cáo không được để trống." } });
            }

            var response = await _blogReportService.EditReportAsync(model.BlogReportId, model.Reason);
            if (response != null && response.IsSuccess)
            {
                return Json(new { isSuccess = true });
            }

            return Json(new
            {
                isSuccess = false,
                message = response?.Message ?? "Không thể cập nhật lý do."
            });
        }

        public async Task<IActionResult> ViewBlogReports(int blogId)
        {
            List<BlogReportDTO> reports = new();
            Dictionary<string, UserDTO> reportAuthors = new();

            var currentUserId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;

            ResponseDTO? response = await _blogReportService.GetReportsByBlogIdAsync(blogId);
            if (response != null && response.IsSuccess)
            {
                reports = JsonConvert.DeserializeObject<List<BlogReportDTO>>(response.Result.ToString());

                var userIds = reports
                    .Where(r => !string.IsNullOrEmpty(r.UserId))
                    .Select(r => r.UserId!)
                    .Distinct()
                    .ToList();

                foreach (var userId in userIds)
                {
                    var userResponse = await _userService.GetUserById(userId);
                    if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                    {
                        var user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                        if (user != null)
                        {
                            reportAuthors[userId] = user;
                        }
                    }
                }

                ViewBag.BlogId = blogId;
                ViewBag.UserId = currentUserId;
                ViewBag.ReportAuthors = reportAuthors; 
                return View(reports);
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể lấy danh sách báo cáo.";
                return RedirectToAction("ViewBlogDetail", "BlogPost", new { id = blogId });
            }
        }


        //Feedback
        [HttpGet]
        public IActionResult SubmitFeedback()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(FeedbackDTO feedback)
        {
            var userid = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if(userid == null){
                TempData["error"] = "Vui lòng đăng nhập";
                return RedirectToAction("Login", "Auth");
            }
            feedback.UserId = userid;

            feedback.CreatedAt = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return View(feedback);
            }


            ResponseDTO? response = await _feedbackService.SubmitFeedback(feedback);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cảm ơn bạn đã gửi phản hồi!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response?.Message ?? "Gửi phản hồi thất bại.";
                return View(feedback);
            }
        }

        public async Task<IActionResult> ViewFeedbacks()
        {
            List<FeedbackWithUserViewModel> feedbacksWithUsers = new();

            ResponseDTO? response = await _feedbackService.GetAllFeedback();
            if (response != null && response.IsSuccess && response.Result != null)
            {
                var feedbackList = JsonConvert.DeserializeObject<List<FeedbackDTO>>(response.Result.ToString());

                foreach (var feedback in feedbackList)
                {
                    UserDTO? user = null;
                    if (!string.IsNullOrEmpty(feedback.UserId))
                    {
                        var userResponse = await _userService.GetUserById(feedback.UserId);
                        if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                        {
                            user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                        }
                    }

                    feedbacksWithUsers.Add(new FeedbackWithUserViewModel
                    {
                        Feedback = feedback,
                        User = user
                    });
                }

                return View(feedbacksWithUsers);
            }

            TempData["error"] = response?.Message ?? "Không thể lấy danh sách phản hồi.";
            return RedirectToAction("Index", "Home");
        }

        //ReportLeader
        [HttpGet]
        public IActionResult SubmitReport()
        {
            return View(); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitReport(ReportDTO report)
        {
            var userid = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            if (userid == null)
            {
                TempData["error"] = "Vui lòng đăng nhập";
                return RedirectToAction("Login", "Auth");
            }
            // Gán UserId từ thông tin đăng nhập
            report.LeaderId = userid;

            report.CreatedAt = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return View(report);
            }


            ResponseDTO? response = await _reportService.SubmitReport(report);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cảm ơn bạn đã gửi phản hồi!";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = response?.Message ?? "Gửi phản hồi thất bại.";
                return View(report);
            }
        }

        public async Task<IActionResult> ViewReports()
        {
            List<ReportWithUserViewModel> reportsWithUsers = new();

            ResponseDTO? response = await _reportService.GetAllReports();
            if (response != null && response.IsSuccess && response.Result != null)
            {
                var reportList = JsonConvert.DeserializeObject<List<ReportDTO>>(response.Result.ToString());

                foreach (var report in reportList)
                {
                    UserDTO? user = null;
                    if (!string.IsNullOrEmpty(report.LeaderId)) 
                    {
                        var userResponse = await _userService.GetUserById(report.LeaderId);
                        if (userResponse != null && userResponse.IsSuccess && userResponse.Result != null)
                        {
                            user = JsonConvert.DeserializeObject<UserDTO>(userResponse.Result.ToString());
                        }
                    }

                    reportsWithUsers.Add(new ReportWithUserViewModel
                    {
                        Report = report,
                        User = user
                    });
                }

                return View(reportsWithUsers);
            }

            TempData["error"] = response?.Message ?? "Không thể lấy danh sách báo cáo.";
            return RedirectToAction("Index", "Home");
        }


        public async Task<IActionResult> MyPendingBlogs()
        {
            List<BlogPostDTO> listBlog = new();

            var userId = User.Claims
                .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                TempData["error"] = "Không thể xác định người dùng. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Auth"); 
            }
            if (!string.IsNullOrEmpty(userId))
            {
                ResponseDTO? response = await _blogPostService.GetBlogCreate(userId);
                if (response != null && response.IsSuccess)
                {
                    listBlog = JsonConvert.DeserializeObject<List<BlogPostDTO>>(response.Result.ToString());
                }
            }

            return View(listBlog);
        }

    }
}
