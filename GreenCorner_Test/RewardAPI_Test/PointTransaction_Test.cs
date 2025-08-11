using GreenCorner.RewardAPI.Controllers;
using GreenCorner.RewardAPI.Models.DTO;
using GreenCorner.RewardAPI.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.RewardAPI_Test
{
    public class PointTransaction_Test
    {
        private readonly Mock<IPointTransactionService> _serviceMock;
        private readonly PointTransactionController _controller;

        public PointTransaction_Test()
        {
            _serviceMock = new Mock<IPointTransactionService>();
            _controller = new PointTransactionController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnSuccess_WhenServiceReturnsData()
        {
            // Arrange
            var fakeData = new[] { new PointTransactionDTO { UserId = "u1", Points = 100 } };
            _serviceMock.Setup(s => s.GetAllPointTransaction())
                        .ReturnsAsync(fakeData);

            // Act
            var result = await _controller.GetAll();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(fakeData, result.Result);
        }

        [Fact]
        public async Task GetAll_ShouldReturnFail_WhenServiceThrows()
        {
            _serviceMock.Setup(s => s.GetAllPointTransaction())
                        .ThrowsAsync(new Exception("DB error"));

            var result = await _controller.GetAll();

            Assert.False(result.IsSuccess);
            Assert.Equal("DB error", result.Message);
        }

        [Fact]
        public async Task GetUserPointTransaction_ShouldReturnData()
        {
            var fakeData = new[] { new PointTransactionDTO { UserId = "u1" } };
            _serviceMock.Setup(s => s.GetByUserId("u1"))
                        .ReturnsAsync(fakeData);

            var result = await _controller.GetUserPointTransaction("u1");
            Assert.True(result.IsSuccess);
            Assert.Equal(fakeData, result.Result);
        }

        [Fact]
        public async Task TransactionPoint_ShouldReturnSuccess()
        {
            _serviceMock.Setup(s => s.TransactionPoint("u1", 50))
                        .Returns(Task.CompletedTask);

            var result = await _controller.TransactionPoint("u1", 50);

            Assert.True(null);
            Assert.Null(result.Message);
        }

        [Fact]
        public async Task TransactionPoints_ShouldReturnSuccess()
        {
            var dto = new PointTransactionDTO
            {
                UserId = "u1",
                Points = 100,
                Type = "Thưởng"
            };
            _serviceMock.Setup(s => s.TransactionPoints("u1", 100, "Thưởng", null))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.TransactionPoints(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Giao dịch Thưởng điểm thành công.", result.Message);
        }

        [Fact]
        public async Task TransactionPoints_ShouldReturnFail_WhenNotEnoughPoints()
        {
            // Arrange
            var dto = new PointTransactionDTO
            {
                UserId = "u1",
                Points = 9999,
                Type = "Đổi"
            };
            _serviceMock.Setup(s => s.TransactionPoints("u1", 9999, "Đổi", null))
                        .ThrowsAsync(new InvalidOperationException());

            // Act
            var result = await _controller.TransactionPoints(dto);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Equal("Bạn không đủ điểm!", result.Message);
        }

        [Fact]
        public async Task GetRewardPoint_ShouldReturnSuccess()
        {
            var result = await _controller.GetRewardPoint("u1");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("", result.Result);
        }

        [Fact]
        public async Task AddTransaction_ShouldReturnSuccess()
        {
            // Arrange
            var dto = new PointTransactionDTO { UserId = "u1" };
            _serviceMock.Setup(s => s.AddTransactionAsync(dto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddTransaction(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Giao dịch đã được thêm.", result.Message);
        }

        [Fact]
        public async Task UpdateRewardPoint_ShouldReturnSuccess()
        {
            // Arrange
            var dto = new RewardPointDTO { UserId = "u1", TotalPoints = 50 };
            _serviceMock.Setup(s => s.UpdateRewardPointAsync(dto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateRewardPoint(dto);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("Điểm thưởng đã được cập nhật.", result.Message);
        }

        [Fact]
        public async Task GetRewardPointHistory_ShouldReturnData()
        {
            // Arrange
            var fakeHistory = new[] { new { Event = "Test Event", TotalPoints = 10 } };

            // Act
            var result = await _controller.GetRewardPointHistory();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(fakeHistory, result.Result);
        }

        [Fact]
        public async Task HasReceivedReward_ShouldReturnTrue()
        {
            // Arrange
            _serviceMock.Setup(s => s.HasReceivedReward("u1", 1))
                        .ReturnsAsync(true);

            // Act
            var result = await _controller.HasReceivedReward("u1", 1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.True((bool)result.Result);
            Assert.Contains("đã nhận điểm", result.Message);
        }

        [Fact]
        public async Task HasReceivedReward_ShouldReturnFalse()
        {
            // Arrange
            _serviceMock.Setup(s => s.HasReceivedReward("u1", 1))
                        .ReturnsAsync(false);

            // Act
            var result = await _controller.HasReceivedReward("u1", 1);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False((bool)result.Result);
            Assert.Contains("chưa nhận điểm", result.Message);
        }
    }
}
