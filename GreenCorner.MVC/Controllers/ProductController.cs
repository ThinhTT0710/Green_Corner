using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace GreenCorner.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IAdminService _adminService;
		public ProductController(IProductService productService, IAdminService adminService)
		{
			_productService = productService;
			_adminService = adminService;
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
					Description = $"Nhân viên {StaffName} đã thêm mới sản phẩm {productDto.Name} với ID {productDto.ProductId}",
					CreatedAt = DateTime.Now,
				};
				var logResponse = await _adminService.AddLogStaff(log);
                return RedirectToAction("ViewAllProduct", "SaleStaff");
            }
            else
            {
                TempData["error"] = response?.Message;
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
    }
}
