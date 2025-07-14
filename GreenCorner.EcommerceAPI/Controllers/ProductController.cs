using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EcommerceAPI.Controllers
{
    [Route("api/Product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ResponseDTO _responseDTO;
        public ProductController(IProductService productService)
        {
            _productService = productService;
            _responseDTO = new ResponseDTO();
        }

        [HttpGet]
        public async Task<ResponseDTO> GetProducts()
        {
            try
            {
                var products = await _productService.GetAllProduct();
                _responseDTO.Result = products;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetByProductId(id);
                _responseDTO.Result = product;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost("search")]
        public async Task<ResponseDTO> Search([FromBody] string keyword)
        {
            try
            {
                var products = await _productService.Search(keyword);
                _responseDTO.Result = products;
                if (products.Count() == 0)
                {
                    _responseDTO.Message = "No product found";
                    _responseDTO.IsSuccess = false;
                    return _responseDTO;
                }
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost]
        public async Task<ResponseDTO> CreateProduct([FromBody] ProductDTO product)
        {
            try
            {
				var createdProduct = await _productService.AddProduct(product);
				_responseDTO.Result = createdProduct;
				return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut]
        public async Task<ResponseDTO> UpdateProduct([FromBody] ProductDTO productDto)
        {
            try
            {
                await _productService.UpdateProduct(productDto);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpDelete("{id}")]
        public async Task<ResponseDTO> DeleteProduct(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("get-newest-products")]
        public async Task<ResponseDTO> GetNewestProducts()
        {
            try
            {
                var newestProducts = await _productService.GetNewestProducts();
                _responseDTO.Result = newestProducts;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("outofstock")]
        public async Task<ResponseDTO> GetOutOfStockProduct()
        {
            try
            {
                var outOfStockProducts = await _productService.GetOutOfStockProduct();
                _responseDTO.Result = outOfStockProducts;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
    }
}
