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
        private readonly IEventService _eventService;
        private readonly IVoucherService _voucherService;
        public HomeController(IOrderService orderService, IProductService productService, IEventService eventService, IVoucherService voucherService)
        {
            _orderService = orderService;
            _productService = productService;
            _eventService = eventService;
            _voucherService = voucherService;
        }

        public async Task<IActionResult> Index()
        {
            List<ProductDTO>? listTrendingProduct = new();
            List<ProductDTO>? listNewestProduct = new();
            List<EventDTO>? listOpenEvents = new();
            List<VoucherDTO>? listTopVouchers = new();

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

            ResponseDTO? responseOpenEvents = await _eventService.GetTop3OpenEventsAsync();
            if (responseOpenEvents != null && responseOpenEvents.IsSuccess)
            {
                listOpenEvents = JsonConvert.DeserializeObject<List<EventDTO>>(responseOpenEvents.Result.ToString());
            }
            else
            {
                TempData["error"] = responseOpenEvents?.Message ?? "L?i khi t?i s? ki?n ?ang m?.";
                return RedirectToAction("Error404", "Home");
            }

            ResponseDTO? responseTopVouchers = await _voucherService.GetTop10ValidVouchersAsync();
            if (responseTopVouchers != null && responseTopVouchers.IsSuccess)
            {
                listTopVouchers = JsonConvert.DeserializeObject<List<VoucherDTO>>(responseTopVouchers.Result.ToString());
            }
            else
            {
                TempData["error"] = responseTopVouchers?.Message ?? "L?i khi t?i danh sách voucher.";
                return RedirectToAction("Error404", "Home");
            }


            HomePageViewModel homePageViewModel = new()
            {
                BestSellingProducts = listTrendingProduct,
                NewestProducts = listNewestProduct,
                Top3OpenEvents = listOpenEvents,
                Top10Vouchers = listTopVouchers
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
