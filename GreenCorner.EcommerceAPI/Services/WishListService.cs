using AutoMapper;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using GreenCorner.EcommerceAPI.Services.Interface;

namespace GreenCorner.EcommerceAPI.Services
{
    public class WishListService : IWishListService
    {
        private readonly IWishListRepository _wishListRepository;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public WishListService(IWishListRepository wishListRepository, IMapper mapper, IProductService productService)
        {
            _wishListRepository = wishListRepository;
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IEnumerable<WishListDTO>> GetAll()
        {
            var wishLists = await _wishListRepository.GetAll();
            return _mapper.Map<IEnumerable<WishListDTO>>(wishLists);
        }

        public async Task<WishListDTO> GetById(int id)
        {
            var wishList = await _wishListRepository.GetById(id);
            return _mapper.Map<WishListDTO>(wishList);
        }

        public async Task Add(WishListDTO item)
        {
            var product = await _productService.GetByProductId(item.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            var wishList = _mapper.Map<WishList>(item);
            await _wishListRepository.Add(wishList);
        }

        public async Task Update(WishListDTO item)
        {
            var product = await _productService.GetByProductId(item.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            var wishList = _mapper.Map<WishList>(item);
            await _wishListRepository.Update(wishList);
        }

        public async Task Delete(int id)
        {
            await _wishListRepository.Delete(id);
        }

        public async Task DeleteByUserId(string userId, int productId)
        {
            await _wishListRepository.DeleteByUserId(userId, productId);
        }

        public async Task<List<WishListDTO>> GetByUserId(string userID)
        {
            var wishLists = await _wishListRepository.GetByUserId(userID);
            return _mapper.Map<List<WishListDTO>>(wishLists);
        }
    }
}
