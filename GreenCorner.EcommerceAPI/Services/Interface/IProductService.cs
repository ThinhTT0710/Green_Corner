using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProduct();
        Task<ProductDTO> GetByProductId(int id);
        Task<IEnumerable<ProductDTO>> Search(string keyword);
        Task<ProductDTO> AddProduct(ProductDTO product);
        Task UpdateProduct(ProductDTO product);
        Task DeleteProduct(int id);
        Task<IEnumerable<ProductDTO>> GetOutOfStockProduct();

    }
}
