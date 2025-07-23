using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAdminService _adminService;
        private readonly IWishListService _wishListService;
        private readonly IOrderService _orderService;
        private readonly IEventService _eventService;
        public ProductController(IProductService productService, IAdminService adminService, IWishListService wishListService, IOrderService orderService, IEventService eventService)
        {
            _productService = productService;
            _adminService = adminService;
            _wishListService = wishListService;
            _orderService = orderService;
            _eventService = eventService;
        }
        public async Task<IActionResult> Index()
        {
            List<ProductDTO> listProduct = new();
            ResponseDTO? response = await _productService.GetAllProduct();
            if (response != null && response.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());
            }
            else 
            {
                TempData["error"] = response?.Message;
            }
            return View(listProduct);
        }

        public async Task<IActionResult> Detail(int id)
		{
            try
            {
                var productResponse = await _productService.GetByProductId(id);
                if (productResponse == null || !productResponse.IsSuccess)
                {
                    return RedirectToAction("Index", "Home");
                }
                var product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(productResponse.Result));

                var viewModel = new ProductDetailViewModel
                {
                    Product = product,
                    IsInWishlist = false ,
                    BestSellingProducts= new List<ProductDTO>(),
                    Top3OpenEvents = new List<EventDTO>(),
                };

                if (User.Identity.IsAuthenticated)
                {
                    var userId = User.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub)?.Value;
                    var wishlistResponse = await _wishListService.GetUserWishList(userId);
                    if (wishlistResponse != null && wishlistResponse.IsSuccess)
                    {
                        var wishListItems = JsonConvert.DeserializeObject<List<WishListDTO>>(Convert.ToString(wishlistResponse.Result));
                        if (wishListItems.Any(w => w.ProductId == id))
                        {
                            viewModel.IsInWishlist = true;
                        }
                    }
                }

                ResponseDTO? response = await _orderService.GetTopBestSelling();
                if (response != null && response.IsSuccess)
                {
                    viewModel.BestSellingProducts = JsonConvert.DeserializeObject<List<ProductDTO>>(response.Result.ToString());
                }
                else
                {
                    TempData["error"] = response.Message == null ? "Error" : response.Message;
                    return RedirectToAction("Error404", "Home");
                }

                ResponseDTO? responseOpenEvents = await _eventService.GetTop3OpenEventsAsync();
                if (responseOpenEvents != null && responseOpenEvents.IsSuccess)
                {
                    viewModel.Top3OpenEvents = JsonConvert.DeserializeObject<List<EventDTO>>(responseOpenEvents.Result.ToString());
                }
                else
                {
                    TempData["error"] = responseOpenEvents?.Message ?? "L?i khi t?i s? ki?n ?ang m?.";
                    return RedirectToAction("Error404", "Home");
                }

                return View(viewModel);
            }
            catch(Exception ex)
            {
				TempData["error"] = ex.Message;
				return RedirectToAction("Index", "Product");
			}
		}

		public async Task<IActionResult> CreateNewProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewProduct(ProductDTO productDTO)
        {
			if (!User.Identity.IsAuthenticated)
			{
				TempData["loginError"] = "Bạn cần đăng nhập để thực hiện được chức năng này";
				return RedirectToAction("Login", "Auth");
			}

			var files = Request.Form.Files;

            var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                files, folderName: "product", filePrefix: "product");

            if (!isSuccess)
            {
                ModelState.AddModelError("Images", errorMessage);
                return View(productDTO);
            }

            productDTO.ImageUrl = string.Join("&", imagePaths);

            if (ModelState.IsValid)
            {
                var response = await _productService.AddProduct(productDTO);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product created successfully!";
					ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                    var StaffName = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Name).FirstOrDefault()?.Value;
                    var log = new SystemLogDTO()
                    {
                        UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value,
                        ActionType = "Thêm mới",
                        ObjectType = "Sản phẩm",
                        ObjectId = product.ProductId,
                        Description = $"Nhân viên {StaffName} đã thêm mới sản phẩm {product.Name} với ID {product.ProductId}",
                        CreatedAt = DateTime.Now,
                    };
					var logResponse = await _adminService.AddLogStaff(log);
                    return RedirectToAction("ViewAllProduct", "SaleStaff");
                }

                TempData["error"] = response?.Message ?? "Có lỗi xảy ra.";
            }

            return View(productDTO);
        }


        public async Task<IActionResult> DeleteProduct(int productId)
        {
                ResponseDTO response = await _productService.GetByProductId(productId);
                if (response != null && response.IsSuccess)
                {
                ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                return View(product);
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDTO productDto)
        {
			if (!User.Identity.IsAuthenticated)
			{
				TempData["loginError"] = "Bạn cần đăng nhập để thực hiện được chức năng này";
				return RedirectToAction("Login", "Auth");
			}
			ResponseDTO response = await _productService.DeleteProduct(productDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Xóa sản phẩm thành công!";
				var StaffName = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Name).FirstOrDefault()?.Value;
				var log = new SystemLogDTO()
				{
					UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value,
					ActionType = "Xóa",
					ObjectType = "Sản phẩm",
					ObjectId = productDto.ProductId,
					Description = $"Nhân viên {StaffName} đã xóa sản phẩm {productDto.Name} với ID {productDto.ProductId}",
					CreatedAt = DateTime.Now,
				};
				var logResponse = await _adminService.AddLogStaff(log);
                return RedirectToAction("ViewAllProduct", "SaleStaff");
            }
            else
            {
                TempData["error"] = response?.Message;
                return RedirectToAction("ProductList", "SaleStaff");
            }
            return View(productDto);
        }

        public async Task<IActionResult> UpdateProduct(int productId)
        {
            ResponseDTO response = await _productService.GetByProductId(productId);
            if (response != null && response.IsSuccess)
            {
                ProductDTO product = JsonConvert.DeserializeObject<ProductDTO>(response.Result.ToString());
                return View(product);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductDTO productDto)
        {
			if (!User.Identity.IsAuthenticated)
			{
				TempData["loginError"] = "Bạn cần đăng nhập để thực hiện được chức năng này";
				return RedirectToAction("Login", "Auth");
			}

			var files = Request.Form.Files;

            bool hasNewImages = files != null && files.Count > 0;

            if (hasNewImages)
            {
                if (!string.IsNullOrEmpty(productDto.ImageUrl))
                {
                    foreach (var oldPath in productDto.ImageUrl.Split("&"))
                    {
                        var fullOldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldPath.TrimStart('/'));
                        if (System.IO.File.Exists(fullOldPath))
                        {
                            System.IO.File.Delete(fullOldPath);
                        }
                    }
                }

                var (isSuccess, imagePaths, errorMessage) = await FileUploadHelper.UploadImagesStrictAsync(
                    files, folderName: "product", filePrefix: "product");

                if (!isSuccess)
                {
                    ModelState.AddModelError("Images", errorMessage);
                    return View(productDto);
                }

                productDto.ImageUrl = string.Join("&", imagePaths);
            }

            var response = await _productService.UpdateProduct(productDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Cập nhật sản phẩm thành công!";
				var StaffName = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Name).FirstOrDefault()?.Value;
				var log = new SystemLogDTO()
				{
					UserId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub).FirstOrDefault()?.Value,
					ActionType = "Cập nhật",
					ObjectType = "Sản phẩm",
					ObjectId = productDto.ProductId,
					Description = $"Nhân viên {StaffName} đã thêm mới sản phẩm {productDto.Name} với ID {productDto.ProductId}",
					CreatedAt = DateTime.Now,
				};
				var logResponse = await _adminService.AddLogStaff(log);
                return RedirectToAction("ViewAllProduct", "SaleStaff");
            }

            TempData["error"] = response?.Message;
            return View(productDto);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            var res = await _productService.Search(keyword);

            if (res != null && res.IsSuccess)
            {
                var products = JsonConvert.DeserializeObject<List<ProductDTO>>(res.Result.ToString());
                return Json(new { success = true, products = products });
            }
            return Json(new { success = false, message = "No product found" });
        }
    }
}
