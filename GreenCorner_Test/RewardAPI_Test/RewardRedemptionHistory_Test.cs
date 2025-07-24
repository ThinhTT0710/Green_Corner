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
    public class RewardRedemptionHistory_Test
    {
        private readonly Mock<IRewardRedemptionHistoryService> _mockService = new();
        private readonly RewardRedemptionHistoryController _controller;

        public RewardRedemptionHistory_Test()
        {
            _controller = new RewardRedemptionHistoryController(_mockService.Object);
        }

        [Fact]
        public async Task GetRewardRedemptionHistory_ValidUser_ReturnsList()
        {
            var history = new List<UserVoucherRedemptionDTO> { new UserVoucherRedemptionDTO { UserId = "u1", VoucherId = 5 } };
            _mockService.Setup(s => s.GetRewardRedemptionHistory("u1")).ReturnsAsync(history);

            var result = await _controller.GetRewardRedemptionHistory("u1");

            Assert.True(result.IsSuccess);
            Assert.Equal(history, result.Result);
        }

        [Fact]
        public async Task SaveRedemption_ValidInput_ReturnsSuccessMessage()
        {
            var dto = new UserVoucherRedemptionDTO { UserId = "u2", VoucherId = 10 };
            _mockService.Setup(s => s.SaveRedemptionAsync(dto.UserId, dto.VoucherId)).Returns(Task.CompletedTask);

            var result = await _controller.SaveRedemption(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Lưu lịch sử đổi điểm thành công.", result.Message);
        }

        [Fact]
        public async Task GetUserRewardRedemption_ReturnsDistinctUserIds()
        {
            var ids = new List<string> { "userA", "userB" };
            _mockService.Setup(s => s.GetDistinctUserIdsRedeemedAsync()).ReturnsAsync(ids);

            var result = await _controller.GetUserRewardRedemption();

            Assert.True(result.IsSuccess);
            Assert.Equal(ids, result.Result);
        }

        [Fact]
        public async Task MarkAsUsed_ValidId_ReturnsSuccessMessage()
        {
            var updated = new UserVoucherRedemptionDTO { IsUsed = true };
            _mockService.Setup(s => s.UpdateIsUsedAsync(1)).ReturnsAsync(updated);

            var result = await _controller.MarkAsUsed(1);

            Assert.Equal(updated, result.Result);
            Assert.Equal("Sử dụng Voucher thành công.", result.Message);
        }

        [Fact]
        public async Task MarkAsUsed_KeyNotFoundException_ReturnsErrorMessage()
        {
            _mockService.Setup(s => s.UpdateIsUsedAsync(999)).ThrowsAsync(new KeyNotFoundException("Voucher not found"));

            var result = await _controller.MarkAsUsed(999);

            Assert.False(result.IsSuccess);
            Assert.Equal("Voucher not found", result.Message);
        }

        [Fact]
        public async Task MarkAsUsed_InvalidOperationException_ReturnsErrorMessage()
        {
            _mockService.Setup(s => s.UpdateIsUsedAsync(500)).ThrowsAsync(new InvalidOperationException("Already used"));

            var result = await _controller.MarkAsUsed(500);

            Assert.False(result.IsSuccess);
            Assert.Equal("Already used", result.Message);
        }
    }
}
