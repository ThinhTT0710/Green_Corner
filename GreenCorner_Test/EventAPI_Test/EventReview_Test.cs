using GreenCorner.EventAPI.Controllers;
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Moq;

namespace GreenCorner_Test.EventAPI_Test
{
    public class EventReview_Test
    {
        private readonly Mock<IEventReviewService> _mockService;
        private readonly EventReviewController _controller;
        private readonly List<EventReviewDTO> _mockEventReviews;


        public EventReview_Test()
        {
            _mockService = new Mock<IEventReviewService>();
            _controller = new EventReviewController(_mockService.Object);
            _mockEventReviews = GetMockEventReviews();
        }

        private static List<EventReviewDTO> GetMockEventReviews()
        {
            return new List<EventReviewDTO>
        {
            new EventReviewDTO
            {
                EventReviewId = 1,
                CleanEventId = 101,
                UserId = "user_001",
                Rating = 5,
                Comment = "Great event! Very well organized.",
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UpdatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new EventReviewDTO
            {
                EventReviewId = 2,
                CleanEventId = 102,
                UserId = "user_002",
                Rating = 3,
                Comment = "It was decent, but could be better.",
                CreatedAt = DateTime.UtcNow.AddDays(-3),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new EventReviewDTO
            {
                EventReviewId = 3,
                CleanEventId = 103,
                UserId = "user_003",
                Rating = 1,
                Comment = "Very disappointing. Poor management!",
                CreatedAt = DateTime.UtcNow.AddDays(-7),
                UpdatedAt = DateTime.UtcNow.AddDays(-6)
            }
        };
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
        public async Task RateEvent_ShouldReturnSuccess()
        {
            var eventReview = _mockEventReviews.First();
            _mockService.Setup(s => s.RateEvent(eventReview)).Returns(Task.CompletedTask);

            var result = await _controller.RateEvent(eventReview);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetEventReviewById_ShouldReturnEventReview()
        {
            _mockService.Setup(s => s.GetEventReviewById(1))
                .ReturnsAsync(_mockEventReviews.FirstOrDefault(r => r.EventReviewId == 1));

            var result = await _controller.GetEventReviewById(1);

            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task EditEventReview_ShouldReturnSuccess()
        {
            var eventReview = _mockEventReviews.First();
            _mockService.Setup(s => s.EditEventReview(eventReview)).Returns(Task.CompletedTask);

            var result = await _controller.EditEventReview(eventReview);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task DeleteEventReview_ShouldReturnSuccess()
        {
            _mockService.Setup(s => s.DeleteEventReview(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteEventReivew(1);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task ViewEventReviewHistory_ShouldReturnHistory()
        {
            _mockService.Setup(s => s.ViewEventReviewHistory("1"))
                .ReturnsAsync(_mockEventReviews.Where(r => r.UserId == "user_001").ToList());

            var result = await _controller.ViewEventReviewHistory("1");

            Assert.NotNull(result.Result);
        }
    }
}