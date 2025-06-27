using GreenCorner.EventAPI.Controllers;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.EventAPI_Test

{
	public class Event_Test
	{
		private readonly Mock<IEventService> _mockService;
		private readonly EventController _controller;
		private readonly List<EventDTO> _mockEvents;

		public Event_Test()
		{
			_mockService = new Mock<IEventService>();
			_controller = new EventController(_mockService.Object);
			_mockEvents = GetMockEvents();
		}

		private static List<EventDTO> GetMockEvents()
		{
			return new List<EventDTO>
		{
			new EventDTO
			{
				CleanEventId = 1,
				Title = "Community Cleanup",
				Description = "A local event for environmental awareness.",
				StartDate = DateTime.UtcNow.AddDays(-5),
				EndDate = DateTime.UtcNow.AddDays(-2),
				MaxParticipants = 50,
				Status = "Completed",
				CreatedAt = DateTime.UtcNow.AddDays(-7)
			},
			new EventDTO
			{
				CleanEventId = 2,
				Title = "Beach Cleanup Drive",
				Description = "An effort to remove plastic waste from the beach.",
				StartDate = DateTime.UtcNow.AddDays(-3),
				EndDate = DateTime.UtcNow.AddDays(-1),
				MaxParticipants = 30,
				Status = "Completed",
				CreatedAt = DateTime.UtcNow.AddDays(-10)
			}
		};
		}

		[Fact]
		public async Task GetCleanupEvents_ShouldReturnEvents()
		{
			_mockService.Setup(s => s.GetAllEvent()).ReturnsAsync(_mockEvents);

			var result = await _controller.GetCleanupEvents();

			Assert.NotNull(result.Result);
		}

		[Fact]
		public async Task GetEventById_ShouldReturnEvent()
		{
			_mockService.Setup(s => s.GetByEventId(1))
				.ReturnsAsync(_mockEvents.Find(e => e.CleanEventId == 1));

			var result = await _controller.GetEventById(1);

			Assert.NotNull(result.Result);
		}
	}
}
