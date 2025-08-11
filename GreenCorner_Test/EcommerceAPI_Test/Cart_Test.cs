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
    public class Cart_Test
    {
        private readonly Mock<ICartService> _mockService = new();
        private readonly CartController _controller;

        public Cart_Test()
        {
            _controller = new CartController(_mockService.Object);
        }

        [Fact]
        public async Task GetCarts_ReturnsAllCarts()
        {
            var carts = new List<CartDTO> { new CartDTO { CartId = 1 } };
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(carts);

            var result = await _controller.GetCarts();

            Assert.True(result.IsSuccess);
            Assert.Equal(carts, result.Result);
        }

        [Fact]
        public async Task GetCartById_ReturnsSingleCart()
        {
            var cart = new CartDTO { CartId = 2 };
            _mockService.Setup(s => s.GetById(2)).ReturnsAsync(cart);

            var result = await _controller.GetCartById(2);

            Assert.True(result.IsSuccess);
            Assert.Equal(cart, result.Result);
        }

        [Fact]
        public async Task AddToCart_ValidQuantity_ReturnsSuccess()
        {
            var dto = new CartDTO { CartId = 3, Quantity = 1 };
            _mockService.Setup(s => s.AddToCart(dto)).Returns(Task.CompletedTask);

            var result = await _controller.AddToCart(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task AddToCart_InvalidQuantity_ReturnsFailMessage()
        {
            var dto = new CartDTO { CartId = 4, Quantity = 0 };

            var result = await _controller.AddToCart(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Quantity must be greater than 0", result.Message);
        }

        [Fact]
        public async Task UpdateCart_ValidQuantity_ReturnsSuccess()
        {
            var dto = new CartDTO { CartId = 5, Quantity = 2 };
            _mockService.Setup(s => s.UpdateCart(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCart(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateCart_InvalidQuantity_ReturnsFailMessage()
        {
            var dto = new CartDTO { CartId = 6, Quantity = 0 };

            var result = await _controller.UpdateCart(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Quantity must be greater than 0", result.Message);
        }

        [Fact]
        public async Task DeleteCart_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(s => s.DeleteCart(7)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteCart(7);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteUserCart_ValidUser_ReturnsSuccess()
        {
            _mockService.Setup(s => s.DeleteUserCart("user1")).Returns(Task.CompletedTask);

            var result = await _controller.DeleteUserCart("user1");

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetUserCart_ValidUser_ReturnsCartList()
        {
            var list = new List<CartDTO> { new CartDTO { UserId = "user2" } };
            _mockService.Setup(s => s.GetByUserId("user2")).ReturnsAsync(list);

            var result = await _controller.GetUserCart("user2");

            Assert.True(result.IsSuccess);
            Assert.Equal(list, result.Result);
        }
        [Fact]
        public async Task AddToCart_InvalidQuantity_ReturnsFail()
        {
            var dto = new CartDTO { CartId = 4, Quantity = 0 };

            var result = await _controller.AddToCart(dto);

            Assert.False(result.IsSuccess);
            Assert.Equal("Quantity must be greater than 0", result.Message);
        }

        [Fact]
        public async Task UpdateCart_ValidQuantity_ReturnsSuccessfully()
        {
            var dto = new CartDTO { CartId = 5, Quantity = 2 };
            _mockService.Setup(s => s.UpdateCart(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCart(dto);

            Assert.True(result.IsSuccess);
        }

    }
}
