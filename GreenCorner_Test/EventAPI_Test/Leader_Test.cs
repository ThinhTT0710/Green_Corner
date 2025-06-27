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
	public class Leader_Test
	{
		private readonly Mock<ILeaderService> _mockService;
		private readonly LeaderController _controller;
		private readonly List<EventVolunteerDTO> _mockVolunteers;

		public Leader_Test()
		{
			_mockService = new Mock<ILeaderService>();
			_controller = new LeaderController(_mockService.Object);
			_mockVolunteers = GetMockVolunteers();
		}

		private List<EventVolunteerDTO> GetMockVolunteers()
		{
			return new List<EventVolunteerDTO>
		{
			new EventVolunteerDTO
			{
				EventVolunteerId = 1,
				CleanEventId = 101,
				UserId = "user1",
				IsTeamLeader = true,
				AttendanceStatus = "Present",
				PointsAwarded = 10,
				JoinDate = DateTime.UtcNow.AddDays(-5),
				Note = "Punctual"
			},
			new EventVolunteerDTO
			{
				EventVolunteerId = 2,
				CleanEventId = 101,
				UserId = "user2",
				IsTeamLeader = false,
				AttendanceStatus = "Absent",
				PointsAwarded = 0,
				JoinDate = DateTime.UtcNow.AddDays(-4),
				Note = "Informed absence"
			}
		};
		}

		[Fact]
		public async Task ViewVolunteerList_ShouldReturnVolunteerList_WhenIdIsValid()
		{
			int eventId = 101;
			_mockService.Setup(s => s.ViewVolunteerList(eventId)).ReturnsAsync(_mockVolunteers);

			var result = await _controller.ViewVolunteerList(eventId);

			Assert.True(result.IsSuccess);
			var list = Assert.IsAssignableFrom<List<EventVolunteerDTO>>(result.Result);
			Assert.Equal(2, list.Count);
			Assert.Equal("user1", list[0].UserId);
		}

		[Fact]
		public async Task ViewVolunteerList_ShouldFail_WhenServiceThrowsException()
		{
			int eventId = 999;
			_mockService.Setup(s => s.ViewVolunteerList(eventId))
						.ThrowsAsync(new Exception("Event not found"));

			var result = await _controller.ViewVolunteerList(eventId);

			Assert.False(result.IsSuccess);
			Assert.Equal("Event not found", result.Message);
		}
	}
}
