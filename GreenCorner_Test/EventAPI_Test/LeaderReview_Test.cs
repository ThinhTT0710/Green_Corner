using GreenCorner.EventAPI.Controllers;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.EventAPI_Test
{
	public class LeaderReview_Test
	{
		private readonly Mock<ILeaderReviewService> _mockService;
		private readonly LeaderReviewController _controller;
		private readonly List<LeaderReviewDTO> _mockLeaderReviews;

		public LeaderReview_Test()
		{
			_mockService = new Mock<ILeaderReviewService>();
			_controller = new LeaderReviewController(_mockService.Object);
			_mockLeaderReviews = GetMockLeaderReviews();
		}

		private static List<LeaderReviewDTO> GetMockLeaderReviews()
		{
			return new List<LeaderReviewDTO>
		{
			new LeaderReviewDTO
			{
				LeaderReviewId = 1,
				CleanEventId = 101,
				LeaderId = "leader_001",
				ReviewerId = "user_001",
				Rating = 5,
				Comment = "Leader was highly responsible and organized.",
				CreatedAt = DateTime.UtcNow.AddDays(-5),
				UpdatedAt = DateTime.UtcNow.AddDays(-2)
			},
			new LeaderReviewDTO
			{
				LeaderReviewId = 2,
				CleanEventId = 102,
				LeaderId = "leader_002",
				ReviewerId = "user_002",
				Rating = 3,
				Comment = "Did an okay job, but could have been better.",
				CreatedAt = DateTime.UtcNow.AddDays(-3),
				UpdatedAt = DateTime.UtcNow.AddDays(-1)
			}
		};
		}

		[Fact]
		public async Task LeaderReview_ShouldReturnSuccess()
		{
			var review = _mockLeaderReviews.First();
			_mockService.Setup(s => s.LeaderReview(review)).Returns(Task.CompletedTask);

			var result = await _controller.LeaderReview(review);

			Assert.True(result.IsSuccess);
		}

		[Fact]
		public async Task GetLeaderReviewById_ShouldReturnReview()
		{
			_mockService.Setup(s => s.GetLeaderReviewById(1))
				.ReturnsAsync(_mockLeaderReviews.FirstOrDefault(r => r.LeaderReviewId == 1));

			var result = await _controller.GetLeaderReviewById(1);

			Assert.NotNull(result.Result);
		}

		[Fact]
		public async Task EditLeaderReview_ShouldReturnSuccess()
		{
			var review = _mockLeaderReviews.First();
			_mockService.Setup(s => s.EditLeaderReview(review)).Returns(Task.CompletedTask);

			var result = await _controller.EditLeaderReview(review);

			Assert.True(result.IsSuccess);
		}

		[Fact]
		public async Task DeleteLeaderReview_ShouldReturnSuccess()
		{
			_mockService.Setup(s => s.DeleteLeaderReview(1)).Returns(Task.CompletedTask);

			var result = await _controller.DeleteLeaderReview(1);

			Assert.True(result.IsSuccess);
		}

		[Fact]
		public async Task ViewLeaderReviewHistory_ShouldReturnHistory()
		{
			_mockService.Setup(s => s.ViewLeaderReviewHistory("leader_001"))
				.ReturnsAsync(_mockLeaderReviews.Where(r => r.LeaderId == "leader_001").ToList());

			var result = await _controller.ViewLeaderReviewHistory("leader_001");

			Assert.NotNull(result.Result);
		}
	}
}
