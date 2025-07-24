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
    public class WishList_Test
    {
        private readonly Mock<IWishListService> _mockService = new();
        private readonly WishListController _controller;

        public WishList_Test()
        {
            _controller = new WishListController(_mockService.Object);
        }

        [Fact]
        public async Task GetWishList_ReturnsList()
        {
            var list = new List<WishListDTO> { new WishListDTO { WishListId = 1 } };
            _mockService.Setup(s => s.GetAll()).ReturnsAsync(list);

            var result = await _controller.GetWishList();

            Assert.True(result.IsSuccess);
            Assert.Equal(list, result.Result);
        }

        [Fact]
        public async Task GetWishListById_ValidId_ReturnsItem()
        {
            var item = new WishListDTO { WishListId = 2 };
            _mockService.Setup(s => s.GetById(2)).ReturnsAsync(item);

            var result = await _controller.GetWishListById(2);

            Assert.True(result.IsSuccess);
            Assert.Equal(item, result.Result);
        }

        [Fact]
        public async Task AddToWishList_ValidInput_ReturnsSuccess()
        {
            var dto = new WishListDTO { WishListId = 3, ProductId = 99 };
            _mockService.Setup(s => s.Add(dto)).Returns(Task.CompletedTask);

            var result = await _controller.AddToWishList(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateWishList_ValidInput_ReturnsSuccess()
        {
            var dto = new WishListDTO { WishListId = 4, ProductId = 100 };
            _mockService.Setup(s => s.Update(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateWishList(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteWishList_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(s => s.Delete(5)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteWishList(5);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetUserWishList_ReturnsUserItems()
        {
            var items = new List<WishListDTO> { new WishListDTO { UserId = "u1" } };
            _mockService.Setup(s => s.GetByUserId("u1")).ReturnsAsync(items);

            var result = await _controller.GetUserWishList("u1");

            Assert.True(result.IsSuccess);
            Assert.Equal(items, result.Result);
        }
    }
}
