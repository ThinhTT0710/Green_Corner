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
    public class BlogReport_Test
    {
        private readonly Mock<IBlogReportService> _mockService = new();
        private readonly BlogReportController _controller;

        public BlogReport_Test()
        {
            _controller = new BlogReportController(_mockService.Object);
        }

        [Fact]
        public async Task CreateReport_ValidInput_ReturnsSuccess()
        {
            var report = new BlogReportDTO { BlogId = 1, Reason = "Spam", UserId = "u123" };
            _mockService.Setup(x => x.CreateReportAsync(report)).Returns(Task.CompletedTask);

            var result = await _controller.CreateReport(report);

            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Message);
        }

        [Fact]
        public async Task GetReportsByBlogId_ValidId_ReturnsReports()
        {
            var reports = new List<BlogReportDTO> { new BlogReportDTO { BlogId = 1, Reason = "Test" } };
            _mockService.Setup(x => x.GetReportsByBlogIdAsync(1)).ReturnsAsync(reports);

            var result = await _controller.GetReportsByBlogId(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(reports, result.Result);
        }

        [Fact]
        public async Task EditReport_ValidIdAndReason_ReturnsUpdatedReport()
        {
            var updated = new BlogReportDTO { BlogReportId = 10, Reason = "Updated Reason" };
            _mockService.Setup(x => x.EditReportAsync(10, "Updated Reason")).ReturnsAsync(updated);

            var result = await _controller.EditReport(10, "Updated Reason");

            Assert.True(result.IsSuccess);
            Assert.Equal(updated, result.Result);
        }

        [Fact]
        public async Task EditReport_ReportNotFound_ReturnsFailure()
        {
            _mockService.Setup(x => x.EditReportAsync(999, "None")).ReturnsAsync((BlogReportDTO)null);

            var result = await _controller.EditReport(999, "None");

            Assert.False(result.IsSuccess);
            Assert.Equal("Không thể sửa báo cáo", result.Message);
        }

        [Fact]
        public async Task GetReportById_ValidId_ReturnsSingleReport()
        {
            var report = new BlogReportDTO { BlogReportId = 5, Reason = "Bad Content" };
            _mockService.Setup(x => x.GetReportById(5)).ReturnsAsync(report);

            var result = await _controller.GetReportById(5);

            Assert.True(result.IsSuccess);
            Assert.Equal(report, result.Result);
        }
    }

}
