using GreenCorner.EcommerceAPI.Models;

namespace GreenCorner.EcommerceAPI.Repositories.Interface
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProduct();
        Task<Product> GetByProductId(int id);
        Task<Product> AddProduct(Product item);
        Task UpdateProduct(Product item);
        Task DeleteProduct(int id);
    }
}
