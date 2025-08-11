using GreenCorner.EventAPI.Controllers;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.EventAPI_Test

{
	public class Event_Test
    {
		
        private readonly Mock<IEventService> _mockService = new();
        private readonly EventController _controller;

        public Event_Test()
        {
            _controller = new EventController(_mockService.Object);
        }

        [Fact]
        public async Task GetCleanupEvents_ReturnsList()
        {
            var mockEvents = new List<EventDTO> { new EventDTO { CleanEventId = 1 } };
            _mockService.Setup(x => x.GetAllEvent()).ReturnsAsync(mockEvents);

            var result = await _controller.GetCleanupEvents();

            Assert.True(result.IsSuccess);
            Assert.Equal(mockEvents, result.Result);
        }

        [Fact]
        public async Task GetOpenCleanupEvents_ReturnsOpenList()
        {
            var mockOpenEvents = new List<EventDTO> { new EventDTO { Status = "Open" } };
            _mockService.Setup(x => x.GetOpenEvent()).ReturnsAsync(mockOpenEvents);

            var result = await _controller.GetOpenCleanupEvents();

            Assert.True(result.IsSuccess);
            Assert.Equal(mockOpenEvents, result.Result);
        }

        [Fact]
        public async Task GetEventById_ValidId_ReturnsEvent()
        {
            var eventDto = new EventDTO { CleanEventId = 5 };
            _mockService.Setup(x => x.GetByEventId(5)).ReturnsAsync(eventDto);

            var result = await _controller.GetEventById(5);

            Assert.True(result.IsSuccess);
            Assert.Equal(eventDto, result.Result);
        }
        

        [Fact]
        public async Task CreateCleanupEvent_ValidDto_ReturnsSuccess()
        {
            var dto = new EventDTO { Title = "Clean the Park" };
            _mockService.Setup(x => x.CreateCleanupEvent(dto)).Returns(Task.CompletedTask);

            var result = await _controller.CreateCleanupEvent(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateCleanupEvent_ValidDto_ReturnsSuccess()
        {
            var dto = new EventDTO { Title = "Update title" };
            _mockService.Setup(x => x.UpdateCleanupEvent(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCleanupEvent(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateCleanupEventStatus_ValidDto_ReturnsSuccess()
        {
            var dto = new EventDTO { Status = "Closed" };
            _mockService.Setup(x => x.UpdateCleanupEventStatus(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateCleanupEventStatus(dto);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CloseCleanupEvent_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(x => x.CloseCleanupEvent(7)).Returns(Task.CompletedTask);

            var result = await _controller.CloseCleanupEvent(7);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task OpenCleanupEvent_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(x => x.OpenCleanupEvent(8)).Returns(Task.CompletedTask);

            var result = await _controller.OpenCleanupEvent(8);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetEventsByIds_ValidList_ReturnsSuccessAndData()
        {
            var ids = new List<int> { 1, 2 };
            var mockData = new List<EventDTO> { new EventDTO { CleanEventId = 1 } };

            _mockService.Setup(x => x.GetEventsByIdsAsync(ids)).ReturnsAsync(mockData);

            var result = await _controller.GetEventsByIds(ids);

            Assert.True(result.IsSuccess);
            Assert.Equal(mockData, result.Result);
            Assert.Equal("Lấy danh sách sự kiện thành công.", result.Message);
        }

        [Fact]
        public async Task GetParticipationInfo_ValidId_ReturnsCorrectCounts()
        {
            _mockService.Setup(x => x.GetEventParticipationInfoAsync(10)).ReturnsAsync((25, 100));

            var result = await _controller.GetParticipationInfo(10);

            Assert.True(result.IsSuccess);

            var json = JsonConvert.SerializeObject(result.Result);
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            Assert.Equal(25, parsed["Current"]);
            Assert.Equal(100, parsed["Max"]);
        }

        [Fact]
        public async Task IsEventFull_ReturnsCorrectResult()
        {
            _mockService.Setup(x => x.IsEventFullAsync(11)).ReturnsAsync(true);

            var result = await _controller.IsEventFull(11);

            Assert.True(result.IsSuccess);

            var json = JsonConvert.SerializeObject(result.Result);
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);

            Assert.True(parsed["IsFull"]);
        }
        [Fact]
        public async Task Get3CleanupEvents_ReturnsTop3List()
        {
            var mockEvents = new List<EventDTO>
        {
            new EventDTO { Title = "E1" },
            new EventDTO { Title = "E2" },
            new EventDTO { Title = "E3" }
        };

            _mockService.Setup(x => x.GetTop3OpenEventsAsync()).ReturnsAsync(mockEvents);

            var result = await _controller.Get3CleanupEvents();

            Assert.True(result.IsSuccess);
            var list = Assert.IsType<List<EventDTO>>(result.Result);
            Assert.Equal(3, list.Count);
        }

        [Fact]
        public async Task DeleteVolunteersByEventId_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(x => x.DeleteVolunteersByEventId(22)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteVolunteersByEventId(22);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task UpdateVolunteerStatusToParticipated_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(x => x.UpdateVolunteerStatusToParticipated(33)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateVolunteerStatusToParticipated(33);

            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task UpdateVolunteerStatusToParticipated_ValidId_ReturnsSuccess_2()
        {
            _mockService.Setup(x => x.UpdateVolunteerStatusToParticipated(33)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateVolunteerStatusToParticipated(33);

            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task GetEventById_InValidId_ReturnsEvent_2()
        {
            var eventDto = new EventDTO { CleanEventId = 1 };
            _mockService.Setup(x => x.GetByEventId(1)).ReturnsAsync(eventDto);

            var result = await _controller.GetEventById(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(eventDto, result.Result);
        }
        [Fact]
        public async Task GetParticipationInfo_ValidId()
        {
            _mockService.Setup(x => x.GetEventParticipationInfoAsync(10)).ReturnsAsync((25, 100));

            var result = await _controller.GetParticipationInfo(10);

            Assert.True(result.IsSuccess);

            var json = JsonConvert.SerializeObject(result.Result);
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            Assert.Equal(25, parsed["Current"]);
            Assert.Equal(100, parsed["Max"]);
        }

        [Fact]
        public async Task IsEventFull_ReturnsValidNoti()
        {
            _mockService.Setup(x => x.IsEventFullAsync(11)).ReturnsAsync(true);

            var result = await _controller.IsEventFull(11);

            Assert.True(result.IsSuccess);

            var json = JsonConvert.SerializeObject(result.Result);
            var parsed = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);

            Assert.True(parsed["IsFull"]);
        }


    }
}
