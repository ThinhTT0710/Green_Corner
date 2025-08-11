using GreenCorner.EcommerceAPI.Controllers;
using GreenCorner.EcommerceAPI.Models.DTO;
using GreenCorner.EcommerceAPI.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.EcommerceAPI_Test
{
	public class Product_Test
	{
		private readonly Mock<IProductService> _mockService;
		private readonly ProductController _controller;
		private readonly List<ProductDTO> _mockProducts;

		public Product_Test()
		{
			_mockService = new Mock<IProductService>();
			_controller = new ProductController(_mockService.Object);
			_mockProducts = GetMockProducts();
		}

		private static List<ProductDTO> GetMockProducts()
		{
			return new List<ProductDTO>
		{
			new ProductDTO
			{
				ProductId = 1,
				Name = "Eco-Friendly Bag",
				Price = 15.99m,
				Quantity = 100,
				ImageUrl = "https://example.com/bag.jpg",
				Description = "Reusable shopping bag made from recycled materials.",
				Discount = 10,
				Category = "Accessories",
				Brand = "GreenLife",
				Origin = "USA",
				IsDeleted = false,
				CreatedAt = DateTime.UtcNow.AddDays(-10),
				UpdatedAt = DateTime.UtcNow.AddDays(-5)
			},
			new ProductDTO
			{
				ProductId = 2,
				Name = "Solar-Powered Lantern",
				Price = 25.99m,
				Quantity = 50,
				ImageUrl = "https://example.com/lantern.jpg",
				Description = "Portable solar-powered lantern for outdoor use.",
				Discount = 5,
				Category = "Electronics",
				Brand = "SunBright",
				Origin = "Germany",
				IsDeleted = false,
				CreatedAt = DateTime.UtcNow.AddDays(-8),
				UpdatedAt = DateTime.UtcNow.AddDays(-3)
			}
		};
		}

		[Fact]
		public async Task GetProducts_ShouldReturnList()
		{
			_mockService.Setup(s => s.GetAllProduct()).ReturnsAsync(_mockProducts);

			var result = await _controller.GetProducts();

			Assert.NotNull(result.Result);
		}

		[Fact]
		public async Task GetProductById_ShouldReturnProduct()
		{
			_mockService.Setup(s => s.GetByProductId(1))
				.ReturnsAsync(_mockProducts.Find(p => p.ProductId == 1));

			var result = await _controller.GetProductById(1);

			Assert.NotNull(result.Result);
		}

		[Fact]
		public async Task UpdateProduct_ShouldReturnSuccess()
		{
			var productDTO = _mockProducts.First();
			_mockService.Setup(s => s.UpdateProduct(productDTO)).Returns(Task.CompletedTask);

			var result = await _controller.UpdateProduct(productDTO);

			Assert.True(result.IsSuccess);
		}

		[Fact]
		public async Task DeleteProduct_ShouldReturnSuccess()
		{
			_mockService.Setup(s => s.DeleteProduct(1)).Returns(Task.CompletedTask);

			var result = await _controller.DeleteProduct(1);

			Assert.True(result.IsSuccess);
		}
        [Fact]
        public async Task GetProductById_ShouldReturnProductValid()
        {
            _mockService.Setup(s => s.GetByProductId(1))
                .ReturnsAsync(_mockProducts.Find(p => p.ProductId == 1));

            var result = await _controller.GetProductById(1);

            Assert.NotNull(result.Result);
        }
    }
}
