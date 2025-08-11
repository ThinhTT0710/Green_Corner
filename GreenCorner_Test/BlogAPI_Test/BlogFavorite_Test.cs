using Moq;
using GreenCorner.BlogAPI.Controllers;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.BlogAPI_Test
{
    public class BlogFavorite_Test
    {
        private readonly Mock<IBlogFavoriteService> _mockService = new();
        private readonly BlogFavoriteController _controller;

        public BlogFavorite_Test()
        {
            _controller = new BlogFavoriteController(_mockService.Object);
        }

        [Fact]
        public async Task AddFavorite_ValidDTO_ReturnsSuccess()
        {
            var dto = new BlogFavoriteAddDTO { BlogId = 1, UserId = "abc" };
            _mockService.Setup(x => x.AddFavoriteAsync(dto)).ReturnsAsync(true);

            var result = await _controller.AddFavorite(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Message);
        }

        [Fact]
        public async Task GetFavoritesByUser_ValidUserId_ReturnFavoriteList()
        {
            var favorites = new List<BlogFavoriteDTO>
        {
            new BlogFavoriteDTO { BlogId = 1, UserId = "user1" }
        };

            _mockService.Setup(x => x.GetFavoritesByUserAsync("abc")).ReturnsAsync(favorites);

            var result = await _controller.GetFavoritesByUser("abc");

            Assert.True(result.IsSuccess);
            var blogs = Assert.IsType<List<BlogFavoriteDTO>>(result.Result);
            Assert.Single(blogs);
        }

        [Fact]
        public async Task RemoveFavorite_ValidBlogIdAndUserId_ReturnsSuccess()
        {
            _mockService.Setup(x => x.RemoveFavoriteAsync(1, "abc")).ReturnsAsync(true);

            var result = await _controller.RemoveFavorite(1, "abc");

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task IsFavorited_ValidParams_ReturnsTrue()
        {
            _mockService.Setup(x => x.IsFavoritedAsync(1, "abc")).ReturnsAsync(true);

            var result = await _controller.IsFavorited(1, "abc");

            Assert.True(result.IsSuccess);
            Assert.True((bool)result.Result);
        }

        [Fact]
        public async Task GetFavoritesByUser_ValidUserId_ReturnsBlogList()
        {
            var favorites = new List<BlogFavoriteDTO>
        {
            new BlogFavoriteDTO { BlogId = 1, UserId = "user1" }
        };

            _mockService.Setup(x => x.GetFavoritesByUserAsync("abc")).ReturnsAsync(favorites);

            var result = await _controller.GetFavoritesByUser("abc");

            Assert.True(result.IsSuccess);
            var blogs = Assert.IsType<List<BlogFavoriteDTO>>(result.Result);
            Assert.Single(blogs);
        }

    }
}
