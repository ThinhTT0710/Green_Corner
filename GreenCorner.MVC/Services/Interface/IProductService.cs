using GreenCorner.MVC.Models;

namespace GreenCorner.MVC.Services.Interface
{
    public interface IProductService
    {
        Task<ResponseDTO?> GetAllProduct();
        Task<ResponseDTO?> GetByProductId(int id);
        Task<ResponseDTO?> Search(string keyword);
        Task<ResponseDTO?> AddProduct(ProductDTO productDto);
        Task<ResponseDTO?> UpdateProduct(ProductDTO productDto);
        Task<ResponseDTO?> DeleteProduct(int id);
    }
}
