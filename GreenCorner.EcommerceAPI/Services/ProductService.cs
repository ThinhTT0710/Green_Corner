using AutoMapper;
using GreenCorner.EcommerceAPI.Models;
using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Repositories.Interface;
using GreenCorner.EcommerceAPI.Services.Interface;

namespace GreenCorner.EcommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task AddProduct(ProductDTO productDto)
        {
            Product product = _mapper.Map<Product>(productDto);
            await _productRepository.AddProduct(product);
        }

        public async Task DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProduct()
        {
            var products =  await _productRepository.GetAllProduct();
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
    }
}
