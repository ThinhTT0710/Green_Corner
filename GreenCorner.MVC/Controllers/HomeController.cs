using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace GreenCorner.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public HomeController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO>? listTrendingProduct = new();
            List<ProductDTO>? listNewestProduct = new();

            ResponseDTO? response = await _orderService.GetTopBestSelling();
            if (response != null && response.IsSuccess)
            {
                listTrendingProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());
            }
            else
            {
                TempData["error"] = response.Message == null ? "Error" : response.Message;
                return RedirectToAction("Error404", "Home");
            }
            ResponseDTO? responseNewestProduct = await _productService.GetNewestProducts();
            if (responseNewestProduct != null && responseNewestProduct.IsSuccess)
            {
                listNewestProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(responseNewestProduct.Result.ToString());
            }
            else
            {
                TempData["error"] = responseNewestProduct.Message == null ? "Error" : responseNewestProduct.Message;
                return RedirectToAction("Error404", "Home");
            }
            HomePageViewModel homePageViewModel = new()
            {
                BestSellingProducts = listTrendingProduct,
                NewestProducts = listNewestProduct
            };
            return View(homePageViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Error404()
        {
            return View();
        }
    }
}
