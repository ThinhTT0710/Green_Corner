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
	public class TrashEvent_Test
	{
		private readonly Mock<ITrashEventService> _mockService;
		private readonly TrashEventController _controller;
		private readonly List<TrashEventDTO> _mockTrashEvents;

		public TrashEvent_Test()
		{
			_mockService = new Mock<ITrashEventService>();
			_controller = new TrashEventController(_mockService.Object);
			_mockTrashEvents = GetMockTrashEvents();
		}

		private static List<TrashEventDTO> GetMockTrashEvents()
		{
			return new List<TrashEventDTO>
		{
			new TrashEventDTO
			{
				TrashReportId = 1,
				UserId = "user_001",
				Location = "Park",
				Description = "Trash accumulation near the benches.",
				ImageUrl = "https://example.com/trash1.jpg",
				Status = "Pending",
				CreatedAt = DateTime.UtcNow.AddDays(-3)
			},
			new TrashEventDTO
			{
				TrashReportId = 2,
				UserId = "user_002",
				Location = "Beach",
				Description = "Plastic bottles and debris found.",
				ImageUrl = "https://example.com/trash2.jpg",
				Status = "Resolved",
				CreatedAt = DateTime.UtcNow.AddDays(-5)
			}
		};
		}

		[Fact]
		public async Task GetTrashEvents_ShouldReturnList()
		{
			_mockService.Setup(s => s.GetAllTrashEvent()).ReturnsAsync(_mockTrashEvents);

			var result = await _controller.GetTrashEvents();

			Assert.NotNull(result.Result);
		}

		[Fact]
		public async Task GetTrashEventById_ShouldReturnTrashEvent()
		{
			_mockService.Setup(s => s.GetByTrashEventId(1))
				.ReturnsAsync(_mockTrashEvents.Find(e => e.TrashReportId == 1));

			var result = await _controller.GetTrashEventById(1);

			Assert.NotNull(result.Result);
		}

		[Fact]
		public async Task CreateTrashEvent_ShouldReturnSuccess()
		{
			var trashEventDTO = _mockTrashEvents.First();
			_mockService.Setup(s => s.AddTrashEvent(trashEventDTO)).Returns(Task.CompletedTask);

			var result = await _controller.CreateTrashEvent(trashEventDTO);

			Assert.True(result.IsSuccess);
		}

		[Fact]
		public async Task UpdateTrashEvent_ShouldReturnSuccess()
		{
			var trashEventDTO = _mockTrashEvents.First();
			_mockService.Setup(s => s.UpdateTrashEvent(trashEventDTO)).Returns(Task.CompletedTask);

			var result = await _controller.UpdateTrashEvent(trashEventDTO);

			Assert.True(result.IsSuccess);
		}

		[Fact]
		public async Task DeleteTrashEvent_ShouldReturnSuccess()
		{
			_mockService.Setup(s => s.DeleteTrashEvent(1)).Returns(Task.CompletedTask);

			var result = await _controller.DeleteTrashEvent(1);

			Assert.True(result.IsSuccess);
		}
	}
}
