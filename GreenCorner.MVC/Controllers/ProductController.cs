using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GreenCorner.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
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
            if(ModelState.IsValid)
            {
                ResponseDTO response = await _productService.AddProduct(productDTO);
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
            ResponseDTO response = await _productService.DeleteProduct(productDto.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product deleted successfully!";
                return RedirectToAction(nameof(Index));
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
            ResponseDTO response = await _productService.UpdateProduct(productDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }
    }
}
