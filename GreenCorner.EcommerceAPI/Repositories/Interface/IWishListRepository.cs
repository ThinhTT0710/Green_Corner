using GreenCorner.EcommerceAPI.Models;

namespace GreenCorner.EcommerceAPI.Repositories.Interface
{
    public interface IWishListRepository
    {
        Task<IEnumerable<WishList>> GetAll();
        Task<WishList> GetById(int id);
        Task Add(WishList item);
        Task Update(WishList item);
        Task Delete(int id);
        Task DeleteByUserId(string userId, int productId);
        Task<IEnumerable<WishList>> GetByUserId(string userID);
    }
}
