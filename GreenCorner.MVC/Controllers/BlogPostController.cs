using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;

namespace GreenCorner.MVC.Controllers
{
    public class BlogPostController : Controller
    {
        private readonly IBlogPostService _blogPostService;
        private readonly IBlogFavoriteService _blogFavoriteService;
        private readonly IBlogReportService _blogReportService;
        private readonly IFeedbackService _feedbackService;
        private readonly IReportService _reportService;
        public BlogPostController(IBlogPostService blogPostService, IBlogFavoriteService blogFavoriteService, IBlogReportService blogReportService, IFeedbackService feedbackService, IReportService reportService)
        {
            this._blogPostService = blogPostService;
            this._blogFavoriteService = blogFavoriteService;
            _blogReportService = blogReportService;
            _feedbackService = feedbackService;
            _reportService = reportService;
        }

        //BlogPost
        public async Task<IActionResult> Index()
        {
            List<BlogPostDTO> listBlog = new();
            List<int> favoriteBlogIds = new();

            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            // Lấy danh sách blog
            ResponseDTO? response = await _blogPostService.GetAllBlogPost();
            if (response != null && response.IsSuccess)
            {
                listBlog = JsonConvert.DeserializeObject<List<BlogPostDTO>>(response.Result.ToString());
            }

            if (!string.IsNullOrEmpty(userId))
            {
                var favResponse = await _blogFavoriteService.GetFavoritesByUserAsync(userId);
                if (favResponse != null && favResponse.IsSuccess)
                {
                    var favorites = JsonConvert.DeserializeObject<List<BlogFavoriteDTO>>(favResponse.Result.ToString());
                    favoriteBlogIds = favorites.Select(f => f.BlogId).ToList();
                }
            }

            ViewBag.FavoriteBlogIds = favoriteBlogIds;

            return View(listBlog);
        }


        public async Task<IActionResult> ViewBlogDetail(int id)
        {
            BlogPostDTO blogPost = null;
            ResponseDTO? response = await _blogPostService.GetByBlogId(id);
            if (response != null && response.IsSuccess)
            {
                blogPost = JsonConvert.DeserializeObject<BlogPostDTO>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không tìm thấy bài viết.";
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        public async Task<IActionResult> CreateBlog()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(BlogPostDTO blogDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO response = await _blogPostService.AddBlog(blogDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(blogDTO);
        }

        public async Task<IActionResult> UpdateBlog(int blogId)
        {
            ResponseDTO response = await _blogPostService.GetByBlogId(blogId);
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
        public async Task<IActionResult> UpdateBlog(BlogPostDTO blogDto)
        {
            ResponseDTO response = await _blogPostService.UpdateBlog(blogDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Blog updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(blogDto);
        }

        public async Task<IActionResult> DeleteBlog(int blogId)
        {
            ResponseDTO response = await _blogPostService.GetByBlogId(blogId);
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
                TempData["success"] = "Blog deleted successfully!";
                return RedirectToAction(nameof(Index));
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
            // Ví dụ lấy UserId trong controller action
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
            // Ví dụ lấy UserId trong controller action
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
            // Ví dụ lấy UserId trong controller action
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
            // Ví dụ lấy UserId trong controller action
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
            var model = new BlogReportDTO { BlogId = blogId };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport(BlogReportDTO reportDTO)
        {
            if (ModelState.IsValid)
            {
                ResponseDTO? response = await _blogReportService.CreateReportAsync(reportDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Đã gửi báo cáo bài viết thành công!";
                    return RedirectToAction("ViewBlogReports", "BlogPost", new { blogId = reportDTO.BlogId });
                }
                else
                {
                    TempData["error"] = response?.Message ?? "Không thể gửi báo cáo.";
                }
            }
            return View(reportDTO);
        }

        [HttpPost]
        public async Task<IActionResult> EditReport(int reportId, string newReason)
        {
            if (string.IsNullOrWhiteSpace(newReason))
            {
                TempData["error"] = "Lý do báo cáo không được để trống.";
                return RedirectToAction("Index", "BlogPost");
            }

            ResponseDTO? response = await _blogReportService.EditReportAsync(reportId, newReason);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật lý do báo cáo thành công!";
                return RedirectToAction("Index", "BlogPost");
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể cập nhật lý do.";
                return RedirectToAction("Index", "BlogPost");
            }
        }

        public async Task<IActionResult> ViewBlogReports(int blogId)
        {
            List<BlogReportDTO> reports = new();

            ResponseDTO? response = await _blogReportService.GetReportsByBlogIdAsync(blogId);
            if (response != null && response.IsSuccess)
            {
                reports = JsonConvert.DeserializeObject<List<BlogReportDTO>>(response.Result.ToString());
                ViewBag.BlogId = blogId;
                return View(reports); // Trả về view hiển thị danh sách báo cáo
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
            return View(); // Trả về form để nhập feedback
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitFeedback(FeedbackDTO feedback)
        {
            var userid = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            // Gán UserId từ thông tin đăng nhập
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
            List<FeedbackDTO> reports = new();

            ResponseDTO? response = await _feedbackService.GetAllFeedback();
            if (response != null && response.IsSuccess)
            {
                reports = JsonConvert.DeserializeObject<List<FeedbackDTO>>(response.Result.ToString());
                return View(reports); // Trả về view hiển thị danh sách báo cáo
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể lấy danh sách báo cáo.";
                return RedirectToAction("Index", "Home");
            }
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
            List<ReportDTO> reports = new();

            ResponseDTO? response = await _reportService.GetAllReports();
            if (response != null && response.IsSuccess)
            {
                reports = JsonConvert.DeserializeObject<List<ReportDTO>>(response.Result.ToString());
                return View(reports); // Trả về view hiển thị danh sách báo cáo
            }
            else
            {
                TempData["error"] = response?.Message ?? "Không thể lấy danh sách báo cáo.";
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
