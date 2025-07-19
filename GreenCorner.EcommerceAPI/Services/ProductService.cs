using AutoMapper;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Repositories;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using GreenCorner.EcommerceAPI.Services.Interface;
using System.Globalization;
using System.Text;

namespace GreenCorner.EcommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper, IOrderDetailRepository orderDetailRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _orderDetailRepository = orderDetailRepository;
        }
        public async Task<ProductDTO> AddProduct(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            var addedProduct = await _productRepository.AddProduct(product);
            return _mapper.Map<ProductDTO>(addedProduct);
        }

        public async Task DeleteProduct(int id)
        {
            bool isInRestrictedOrder = await _orderDetailRepository.HasRestrictedOrdersByProductId(id);
            if (isInRestrictedOrder)
            {
                throw new Exception("Không thể xóa sản phẩm vì có đơn hàng liên quan đang chờ xác nhận.");
            }

            await _productRepository.DeleteProduct(id);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProduct()
        {
            var products = await _productRepository.GetAllProduct();
            return _mapper.Map<List<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByProductId(int id)
        {
            var product = await _productRepository.GetByProductId(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task UpdateProduct(ProductDTO productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            await _productRepository.UpdateProduct(product);
        }
        public async Task<IEnumerable<ProductDTO>> GetNewestProducts()
        {
            var newestProducts = await _productRepository.GetNewestProducts();
            return _mapper.Map<List<ProductDTO>>(newestProducts);
        }

        public async Task<IEnumerable<ProductDTO>> GetOutOfStockProduct()
        {
            var outOfStockProducts = await _productRepository.GetOutOfStockProduct();
            return _mapper.Map<List<ProductDTO>>(outOfStockProducts);
        }

        public async Task<IEnumerable<ProductDTO>> Search(string keyword)
        {
            var products = await _productRepository.GetAllProduct();
            var searchedProducts = _mapper.Map<List<ProductDTO>>(products);
            if (string.IsNullOrEmpty(keyword))
            {
                throw new Exception("No product found");
            }

            string normalizedKeyword = RemoveDiacritics(keyword.ToLower());
            return searchedProducts.Where(p => RemoveDiacritics(p.Name.ToLower()).Contains(normalizedKeyword));
        }

        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;

            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
