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
    public class RewardPoint_Test
    {
        
    private readonly Mock<IRewardPointService> _mockService = new();
        private readonly RewardPointController _controller;

        public RewardPoint_Test()
        {
            _controller = new RewardPointController(_mockService.Object);
        }

        [Fact]
        public async Task GetRewardPoints_ReturnsRewardList()
        {
            var rewards = new List<RewardPointDTO>
        {
            new RewardPointDTO { UserId = "u1", TotalPoints = 500 },
            new RewardPointDTO { UserId = "u2", TotalPoints = 300 }
        };

            _mockService.Setup(s => s.GetRewardPoints()).ReturnsAsync(rewards);

            var result = await _controller.GetRewardPoints();

            Assert.True(result.IsSuccess);
            Assert.Equal(rewards, result.Result);
        }

        [Fact]
        public async Task AwardPointsToUser_ValidInput_ReturnsSuccess()
        {
            _mockService.Setup(s => s.AwardPointsToUser("u3", 100)).Returns(Task.CompletedTask);

            var result = await _controller.AwardPointsToUser("u3", 100);

            Assert.True(result.IsSuccess);
            Assert.Equal("Success", result.Message);
        }

        [Fact]
        public async Task GetUserTotalPoints_ReturnsCorrectRewardPoint()
        {
            var reward = new RewardPointDTO { UserId = "u4", TotalPoints = 900 };
            _mockService.Setup(s => s.GetUserTotalPoints("u4")).ReturnsAsync(reward);

            var result = await _controller.GetUserTotalPoints("u4");

            Assert.True(result.IsSuccess);
            Assert.Equal(reward, result.Result);
        }
    }
}
