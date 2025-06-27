using AutoMapper;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using GreenCorner.EcommerceAPI.Services.Interface;

namespace GreenCorner.EcommerceAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductService _productService;
		private readonly IMapper _mapper;

		public CartService(ICartRepository cartRepository, IMapper mapper, IProductService productService)
		{
			_cartRepository = cartRepository;
			_mapper = mapper;
			_productService = productService;
		}

		public async Task AddToCart(CartDTO cartDTO)
        {
            var product = await _productService.GetByProductId(cartDTO.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            else if (product.Quantity < cartDTO.Quantity)
            {
                throw new Exception("Quantity of product is not enough");
            }
            else if (product.Quantity == 0)
            {
                throw new Exception("Product is out of stock");
            }
            else
            {
                var existingCartItem = await _cartRepository.GetCartItem(cartDTO.UserId, cartDTO.ProductId);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity += cartDTO.Quantity;
                    await _cartRepository.UpdateCart(existingCartItem);
                }
                else
                {
                    Cart cart = _mapper.Map<Cart>(cartDTO);
                    await _cartRepository.AddToCart(cart);
                }
            }
        }

        public async Task DeleteCart(int id)
        {
            await _cartRepository.DeleteCart(id);
        }

		public async Task DeleteUserCart(string userId)
		{
			await _cartRepository.DeleteUserCart(userId);
		}

		public async Task<IEnumerable<CartDTO>> GetAll()
        {
            var carts = await _cartRepository.GetAll();
            return _mapper.Map<List<CartDTO>>(carts);
        }

        public async Task<CartDTO> GetById(int id)
        {
            var cart = await _cartRepository.GetById(id);
            return _mapper.Map<CartDTO>(cart);
        }

        public async Task<IEnumerable<CartDTO>> GetByUserId(string userId)
        {
            var carts = await _cartRepository.GetByUserId(userId);
            return _mapper.Map<IEnumerable<CartDTO>>(carts);
        }

        public async Task UpdateCart(CartDTO cartDTO)
        {
            var product = await _productService.GetByProductId(cartDTO.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }
            else if (product.Quantity < cartDTO.Quantity)
            {
                throw new Exception("Quantity of product is not enough");
            }
            else if (product.Quantity == 0)
            {
                throw new Exception("Product is out of stock");
            }
            else
            {
                Cart cart = _mapper.Map<Cart>(cartDTO);
                await _cartRepository.UpdateCart(cart);
            }
        }
    }
}
