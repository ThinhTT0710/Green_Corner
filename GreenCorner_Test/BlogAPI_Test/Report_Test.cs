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
    public  class Report_Test
    {
        private readonly Mock<IReportService> _mockService = new();
        private readonly ReportController _controller;

        public Report_Test()
        {
            _controller = new ReportController(_mockService.Object);
        }

        [Fact]
        public async Task SubmitReport_ValidInput_ReturnsSuccessMessage()
        {
            var report = new ReportDTO { LeaderId = "u1", Content = "Vi phạm quy định", Title = "Bài viết ABC" };
            _mockService.Setup(s => s.SubmitReport(report)).Returns(Task.CompletedTask);

            var result = await _controller.SubmitReport(report);

            Assert.True(result.IsSuccess);
            Assert.Equal("Báo cáo đã được gửi thành công.", result.Message);
        }

        [Fact]
        public async Task SubmitReport_ServiceThrowsException_ReturnsFailureMessage()
        {
            var report = new ReportDTO { LeaderId = "u2", Content = "Spam", Title = "Blog XYZ" };
            _mockService.Setup(s => s.SubmitReport(report)).ThrowsAsync(new Exception("test error"));

            var result = await _controller.SubmitReport(report);

            Assert.False(result.IsSuccess);
            Assert.Equal("Báo cáo gửi thất bại!", result.Message);
        }

        [Fact]
        public async Task GetAllReports_ReturnsListOfReports()
        {
            var reports = new List<ReportDTO>
        {
            new ReportDTO { LeaderId = "u1", Content = "Spam" },
            new ReportDTO { LeaderId = "u2", Content = "Vi phạm" }
        };

            _mockService.Setup(s => s.GetAllReports()).ReturnsAsync(reports);

            var result = await _controller.GetAllReports();

            Assert.True(result.IsSuccess);
            var list = Assert.IsType<List<ReportDTO>>(result.Result);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetAllReports_ServiceThrowsException_ReturnsFailureMessage()
        {
            _mockService.Setup(s => s.GetAllReports()).ThrowsAsync(new Exception("error"));

            var result = await _controller.GetAllReports();

            Assert.False(result.IsSuccess);
            Assert.Equal("Không thể lấy danh sách báo cáo!", result.Message);
        }
        [Fact]
        public async Task GetAllReports_ReturnsListOfReportsWithUser()
        {
            var reports = new List<ReportDTO>
        {
            new ReportDTO { LeaderId = "u1", Content = "Spam" },
            new ReportDTO { LeaderId = "u2", Content = "Vi phạm" }
        };

            _mockService.Setup(s => s.GetAllReports()).ReturnsAsync(reports);

            var result = await _controller.GetAllReports();

            Assert.True(result.IsSuccess);
            var list = Assert.IsType<List<ReportDTO>>(result.Result);
            Assert.Equal(2, list.Count);
        }
    }
}
