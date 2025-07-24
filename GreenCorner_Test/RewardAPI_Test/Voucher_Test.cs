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
    public class Voucher_Test
    {
        private readonly Mock<IVoucherService> _mockService = new();
        private readonly VoucherController _controller;

        public Voucher_Test()
        {
            _controller = new VoucherController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllVouchers_ReturnsList()
        {
            var mockList = new List<VoucherDTO> { new VoucherDTO { VoucherId = 1 } };
            _mockService.Setup(s => s.GetAllVouchers()).ReturnsAsync(mockList);

            var result = await _controller.GetAllVouchers();

            Assert.True(result.IsSuccess);
            Assert.Equal(mockList, result.Result);
        }

        [Fact]
        public async Task GetVoucherById_ReturnsVoucher()
        {
            var voucher = new VoucherDTO { VoucherId = 2, Title = "Free Coffee" };
            _mockService.Setup(s => s.GetRewardDetail(2)).ReturnsAsync(voucher);

            var result = await _controller.GetVoucherById(2);

            Assert.True(result.IsSuccess);
            Assert.Equal(voucher, result.Result);
        }

        [Fact]
        public async Task CreateVoucher_ValidVoucher_ReturnsSuccess()
        {
            var dto = new VoucherDTO { VoucherId = 3 };
            _mockService.Setup(s => s.CreateVoucher(dto)).Returns(Task.CompletedTask);

            var result = await _controller.CreateVoucher(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateVoucher_ValidDto_ReturnsSuccess()
        {
            var dto = new VoucherDTO { VoucherId = 4 };
            _mockService.Setup(s => s.UpdateVoucher(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateVoucher(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteVoucher_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(s => s.DeleteVoucher(5)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteVoucher(5);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetAllRewards_ReturnsRewardList()
        {
            var rewards = new List<VoucherDTO> { new VoucherDTO { VoucherId = 6 } };
            _mockService.Setup(s => s.GetAllVouchers()).ReturnsAsync(rewards);

            var result = await _controller.GetAllRewards();

            Assert.True(result.IsSuccess);
            Assert.Equal(rewards, result.Result);
        }

        [Fact]
        public async Task GetTop10Vouchers_ReturnsTop10()
        {
            var top10 = Enumerable.Range(1, 10).Select(i => new VoucherDTO { VoucherId = i }).ToList();
            _mockService.Setup(s => s.GetTop10ValidVouchersAsync()).ReturnsAsync(top10);

            var result = await _controller.GetTop10Vouchers();

            Assert.True(result.IsSuccess);
            var data = Assert.IsType<List<VoucherDTO>>(result.Result);
            Assert.Equal(10, data.Count);
        }

        [Fact]
        public async Task RedeemVoucher_Available_ReturnsSuccessMessage()
        {
            _mockService.Setup(s => s.RedeemVoucherAsync(100)).ReturnsAsync(true);

            var result = await _controller.RedeemVoucher(100);

            Assert.True(result.IsSuccess);
            Assert.Equal("Đổi voucher thành công!", result.Message);
        }

        [Fact]
        public async Task RedeemVoucher_Unavailable_ReturnsFailureMessage()
        {
            _mockService.Setup(s => s.RedeemVoucherAsync(200)).ReturnsAsync(false);

            var result = await _controller.RedeemVoucher(200);

            Assert.False(result.IsSuccess);
            Assert.Equal("Voucher không khả dụng hoặc đã hết.", result.Message);
        }

        [Fact]
        public async Task CleanUpExpiredOrEmptyVouchers_ReturnsMessage()
        {
            _mockService.Setup(s => s.CleanUpExpiredOrEmptyVouchersAsync()).Returns(Task.CompletedTask);

            var result = await _controller.CleanUpExpiredOrEmptyVouchers();

            Assert.True(result.IsSuccess);
            Assert.Equal("Dọn dẹp thành công các voucher hết hạn hoặc đã hết số lượng.", result.Message);
        }


    }
}
