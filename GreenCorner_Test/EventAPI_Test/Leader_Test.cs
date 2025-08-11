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
        private readonly Mock<ILeaderService> _mockService = new();
        private readonly LeaderController _controller;

        public Leader_Test()
        {
            _controller = new LeaderController(_mockService.Object);
        }
        [Fact]
        public async Task AttendanceCheck_ValidParams_ReturnsSuccess()
        {
            _mockService.Setup(s => s.AttendanceCheck("u1", 100, true)).Returns(Task.CompletedTask);

            var result = await _controller.AttendanceCheck("u1", 100, true);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task EditAttendance_ValidParams_ReturnsSuccess()
        {
            _mockService.Setup(s => s.EditAttendance("u1", 100)).Returns(Task.CompletedTask);

            var result = await _controller.EditAttendance("u1", 100);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetEventByLeader_ValidUser_ReturnsListOfEvents()
        {
            var mockEvents = new List<EventDTO> { new EventDTO { Title = "Clean Up" } };
            _mockService.Setup(s => s.GetOpenEventsByTeamLeader("leader1")).ReturnsAsync(mockEvents);

            var result = await _controller.GetEventByLeader("leader1");

            Assert.True(result.IsSuccess);
            Assert.Equal(mockEvents, result.Result);
        }

        [Fact]
        public async Task KickVolunteer_ValidIds_ReturnsSuccess()
        {
            _mockService.Setup(s => s.KickVolunteer("u1", 200)).Returns(Task.CompletedTask);

            var result = await _controller.KickVolunteer("u1", 200);

            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task GetEventByLeader_ValidUser_ReturnsListOfEvents_2()
        {
            var mockEvents = new List<EventDTO> { new EventDTO { Title = "Clean Up" } };
            _mockService.Setup(s => s.GetOpenEventsByTeamLeader("leader1")).ReturnsAsync(mockEvents);

            var result = await _controller.GetEventByLeader("leader1");

            Assert.True(result.IsSuccess);
            Assert.Equal(mockEvents, result.Result);
        }
    }
}
