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
    public  class Feedback_Test
    {
        private readonly Mock<IFeedbackService> _mockService = new();
        private readonly FeedbackController _controller;

        public Feedback_Test()
        {
            _controller = new FeedbackController(_mockService.Object);
        }

        [Fact]
        public async Task SubmitFeedback_ValidInput_ReturnsSuccessMessage()
        {
            var feedback = new FeedbackDTO { Content = "App rất tuyệt!", UserId = "user123" };
            _mockService.Setup(s => s.SubmitFeedback(feedback)).Returns(Task.CompletedTask);

            var result = await _controller.SubmitFeedback(feedback);

            Assert.True(result.IsSuccess);
            Assert.Equal("Phản hồi đã được gửi thành công.", result.Message);
        }

        [Fact]
        public async Task SubmitFeedback_ServiceThrowsException_ReturnsFailure()
        {
            var feedback = new FeedbackDTO { Content = "Bug xảy ra!", UserId = "user456" };
            _mockService.Setup(s => s.SubmitFeedback(feedback)).ThrowsAsync(new Exception("error"));

            var result = await _controller.SubmitFeedback(feedback);

            Assert.False(result.IsSuccess);
            Assert.Equal("Phản hồi gửi thất bại!", result.Message);
        }

        [Fact]
        public async Task GetAllFeedback_ReturnsListSuccessfully()
        {
            var feedbacks = new List<FeedbackDTO>
        {
            new FeedbackDTO { Content = "Hay!", UserId = "u1" },
            new FeedbackDTO { Content = "Chưa tốt", UserId = "u2" }
        };

            _mockService.Setup(s => s.GetAllFeedback()).ReturnsAsync(feedbacks);

            var result = await _controller.GetAllFeedback();

            Assert.True(result.IsSuccess);
            var list = Assert.IsType<List<FeedbackDTO>>(result.Result);
            Assert.Equal(2, list.Count);
        }

        [Fact]
        public async Task GetAllFeedback_ServiceThrowsException_ReturnsFailure()
        {
            _mockService.Setup(s => s.GetAllFeedback()).ThrowsAsync(new Exception("error"));

            var result = await _controller.GetAllFeedback();

            Assert.False(result.IsSuccess);
            Assert.Equal("Lấy danh sách phản hồi thất bại!", result.Message);

        }
        [Fact]
        public async Task GetAllFeedback_ReturnsListSuccessfullyWithUser()
        {
            var feedbacks = new List<FeedbackDTO>
        {
            new FeedbackDTO { Content = "Hay!", UserId = "u1" },
            new FeedbackDTO { Content = "Chưa tốt", UserId = "u2" }
        };

            _mockService.Setup(s => s.GetAllFeedback()).ReturnsAsync(feedbacks);

            var result = await _controller.GetAllFeedback();

            Assert.True(result.IsSuccess);
            var list = Assert.IsType<List<FeedbackDTO>>(result.Result);
            Assert.Equal(2, list.Count);
        }

    }
}