using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllProduct();
        Task<ProductDTO> GetByProductId(int id);
        Task<ProductDTO> AddProduct(ProductDTO product);
        Task UpdateProduct(ProductDTO product);
        Task DeleteProduct(int id);
    }
}
