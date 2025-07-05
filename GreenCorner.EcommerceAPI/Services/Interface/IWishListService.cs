using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;

namespace GreenCorner.EcommerceAPI.Services.Interface
{
    public interface IWishListService
    {
        Task<IEnumerable<WishListDTO>> GetAll();
        Task<WishListDTO> GetById(int id);
        Task Add(WishListDTO item);
        Task Update(WishListDTO item);
        Task Delete(int id);
        Task<List<WishListDTO>> GetByUserId(string userID);
    }
}
