using GreenCorner.MVC.Models;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.Utility;

namespace GreenCorner.MVC.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDTO?> AddProduct(ProductDTO productDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Data = productDto,
                Url = SD.EcommerceAPIBase + "/api/Product"
            });
        }

        public async Task<ResponseDTO?> DeleteProduct(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.DELETE,
                Url = SD.EcommerceAPIBase + "/api/Product/" + id
            });
        }

        public async Task<ResponseDTO?> GetAllProduct()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/Product"
            });
        }

        public async Task<ResponseDTO?> GetByProductId(int id)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/Product/" + id
            });
        }
        public async Task<ResponseDTO?> Search(string keyword)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.POST,
                Url = SD.EcommerceAPIBase + "/api/Product/Search",
                Data = keyword
            });
        }

        public async Task<ResponseDTO?> UpdateProduct(ProductDTO productDto)
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.PUT,
                Data = productDto,
                Url = SD.EcommerceAPIBase + "/api/Product"
            });
        }
        public async Task<ResponseDTO?> OutOfStockProduct()
        {
            return await _baseService.SendAsync(new RequestDTO
            {
                APIType = SD.APIType.GET,
                Url = SD.EcommerceAPIBase + "/api/Product/outofstock"
            });
        }
    }
}
