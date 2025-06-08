using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
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
        public EventStaffController(IBlogPostService blogPostService, IBlogFavoriteService blogFavoriteService, IBlogReportService blogReportService, IFeedbackService feedbackService, IReportService reportService)
        {
            this._blogPostService = blogPostService;
            this._blogFavoriteService = blogFavoriteService;
            _blogReportService = blogReportService;
            _feedbackService = feedbackService;
            _reportService = reportService;
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
                TempData["error"] = response?.Message ?? "Không tìm thấy bài viết.";
                return RedirectToAction("Index");
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

    }
}
