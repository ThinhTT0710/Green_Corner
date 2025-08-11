using GreenCorner.BlogAPI.Controllers;
using GreenCorner.BlogAPI.Models.DTOs;
using GreenCorner.BlogAPI.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.BlogAPI_Test
{
    public class BlogPost_Test
    {
        private readonly Mock<IBlogPostService> _mockService = new();
        private readonly BlogPostController _controller;

        public BlogPost_Test()
        {
            _controller = new BlogPostController(_mockService.Object);
        }

        [Fact]
        public async Task ViewBlogPosts_ReturnsAllPosts()
        {
            var blogs = new List<BlogPostDTO> { new BlogPostDTO { BlogId = 1, Title = "Test Blog" } };
            _mockService.Setup(s => s.GetAllBlogPost()).ReturnsAsync(blogs);

            var result = await _controller.ViewBlogPosts();

            Assert.True(result.IsSuccess);
            Assert.Equal(blogs, result.Result);
        }

        [Fact]
        public async Task ViewPendingPosts_ReturnsPendingPosts()
        {
            var pending = new List<BlogPostDTO> { new BlogPostDTO { BlogId = 2, Status = "Pending" } };
            _mockService.Setup(s => s.GetAllPendingPost()).ReturnsAsync(pending);

            var result = await _controller.ViewPendingPosts();

            Assert.True(result.IsSuccess);
            Assert.Equal(pending, result.Result);
        }

        [Fact]
        public async Task ViewBlogDetails_ValidId_ReturnsBlog()
        {
            var blog = new BlogPostDTO { BlogId = 3, Title = "Chi tiết blog" };
            _mockService.Setup(s => s.GetByBlogId(3)).ReturnsAsync(blog);

            var result = await _controller.ViewBlogDetails(3);

            Assert.True(result.IsSuccess);
            Assert.Equal(blog, result.Result);
        }

        [Fact]
        public async Task CreateBlog_ValidBlog_ReturnsSuccess()
        {
            var dto = new BlogPostDTO { BlogId = 4, Title = "Blog mới" };
            _mockService.Setup(s => s.AddBlog(dto)).Returns(Task.CompletedTask);

            var result = await _controller.CreateBlog(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateBlog_ValidDto_ReturnsSuccess()
        {
            var dto = new BlogPostDTO { BlogId = 5, Title = "Blog cập nhật" };
            _mockService.Setup(s => s.UpdateBlog(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateBlog(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteBlog_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(s => s.DeleteBlog(6)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteBlog(6);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task BlogApproval_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(s => s.BlogApproval(7)).Returns(Task.CompletedTask);

            var result = await _controller.BlogApproval(7);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ViewBlogCreate_ValidUserId_ReturnsCreatedPosts()
        {
            var blogs = new List<BlogPostDTO> { new BlogPostDTO { BlogId = 8, Title = "Blog đã tạo" } };
            _mockService.Setup(s => s.GetBlogCreate("user123")).ReturnsAsync(blogs);

            var result = await _controller.ViewBlogCreate("user123");

            Assert.True(result.IsSuccess);
            Assert.Equal(blogs, result.Result);
        }
        [Fact]
        public async Task UpdateBlog_ValidDto_ReturnsSuccessfully()
        {
            var dto = new BlogPostDTO { BlogId = 5, Title = "Blog cập nhật" };
            _mockService.Setup(s => s.UpdateBlog(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateBlog(dto);

            Assert.True(result.IsSuccess);
        }
    }

}
