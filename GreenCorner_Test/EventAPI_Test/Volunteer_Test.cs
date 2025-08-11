using GreenCorner.AuthAPI.Models.DTO;
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
    public class Volunteer_Test
    {
        
    private readonly Mock<IVolunteerService> _mockService = new();
        private readonly VolunteerController _controller;

        public Volunteer_Test()
        {
            _controller = new VolunteerController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsVolunteers()
        {
            var mockData = new List<VolunteerDTO> { new VolunteerDTO { UserId = "u1" } };
            _mockService.Setup(s => s.GetAllVolunteer()).ReturnsAsync(mockData);

            var result = await _controller.GetAll();

            Assert.True(result.IsSuccess);
            Assert.Equal(mockData, result.Result);
        }

        [Fact]
        public async Task RegisterVolunteer_ValidDto_ReturnsSuccess()
        {
            var dto = new VolunteerDTO { UserId = "u1" };
            _mockService.Setup(s => s.RegisterVolunteer(dto)).ReturnsAsync("Đăng ký thành công");

            var result = await _controller.RegisterVolunteer(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Đăng ký thành công", result.Message);
        }

        [Fact]
        public async Task UnRegisterVolunteer_ValidInput_ReturnsSuccess()
        {
            _mockService.Setup(s => s.UnregisterAsync(1, "u1", "Volunteer")).ReturnsAsync("Hủy thành công");

            var result = await _controller.UnRegisterVolunteer(1, "u1", "Volunteer");

            Assert.True(result.IsSuccess);
            Assert.Equal("Hủy thành công", result.Message);
        }

        [Fact]
        public async Task IsVolunteer_Confirmed_ReturnsMessage()
        {
            _mockService.Setup(s => s.IsVolunteer(1, "u1")).ReturnsAsync(true);

            var result = await _controller.IsVolunteer(1, "u1");

            Assert.True(result.IsSuccess);
            Assert.Equal("Bạn đã đăng ký, vui lòng đợi phê duyệt.", result.Message);
        }

        [Fact]
        public async Task IsTeamLeader_NotRegistered_ReturnsFalse()
        {
            _mockService.Setup(s => s.IsTeamLeader(2, "u2")).ReturnsAsync(false);

            var result = await _controller.IsTeamLeader(2, "u2");

            Assert.False(result.IsSuccess);
            Assert.Equal("Chưa đăng ký!", result.Message);
        }

        [Fact]
        public async Task UpdateRegister_ValidDto_ReturnsSuccess()
        {
            var dto = new VolunteerDTO { UserId = "u3" };
            _mockService.Setup(s => s.UpdateRegister(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateRegister(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Cập nhật đăng ký thành công.", result.Message);
        }

        [Fact]
        public async Task GetAllVolunteerRegistrations_ReturnsList()
        {
            var list = new List<VolunteerDTO> { new VolunteerDTO { UserId = "u4" } };
            _mockService.Setup(s => s.GetAllVolunteerRegistrations()).ReturnsAsync(list);

            var result = await _controller.GetAllVolunteerRegistrations();

            Assert.True(result.IsSuccess);
            Assert.Equal("Lấy danh sách tình nguyện viên thành công.", result.Message);
        }

        [Fact]
        public async Task GetVolunteerRegistrationById_ValidId_ReturnsSuccess()
        {
            var dto = new VolunteerDTO { UserId = "u5" };
            _mockService.Setup(s => s.GetVolunteerRegistrationById(5)).ReturnsAsync(dto);

            var result = await _controller.GetVolunteerRegistrationById(5);

            Assert.True(result.IsSuccess);
            Assert.Equal("Lấy chi tiết tình nguyện viên thành công.", result.Message);
        }

        [Fact]
        public async Task ApproveVolunteer_ValidId_ReturnsSuccess()
        {
            _mockService.Setup(s => s.ApproveVolunteerRegistration(6)).Returns(Task.CompletedTask);

            var result = await _controller.ApproveVolunteer(6);

            Assert.True(result.IsSuccess);
            Assert.Equal("Phê duyệt tình nguyện viên thành công.", result.Message);
        }
    
    [Fact]
    public async Task GetAllTeamLeaderRegistrations_ReturnsList()
    {
        var list = new List<VolunteerDTO> { new VolunteerDTO { UserId = "leader1" } };
        _mockService.Setup(s => s.GetAllTeamLeaderRegistrations()).ReturnsAsync(list);

        var result = await _controller.GetAllTeamLeaderRegistrations();

        Assert.True(result.IsSuccess);
        Assert.Equal("Lấy danh sách trưởng nhóm thành công.", result.Message);
        Assert.Equal(list, result.Result);
    }

    [Fact]
    public async Task GetTeamLeaderRegistrationById_ValidId_ReturnsSuccess()
    {
        var dto = new VolunteerDTO { UserId = "leader2" };
        _mockService.Setup(s => s.GetTeamLeaderRegistrationById(5)).ReturnsAsync(dto);

        var result = await _controller.GetTeamLeaderRegistrationById(5);

        Assert.True(result.IsSuccess);
        Assert.Equal("Lấy chi tiết trưởng nhóm thành công.", result.Message);
        Assert.Equal(dto, result.Result);
    }

    [Fact]
    public async Task ApproveTeamLeader_ValidId_ReturnsSuccess()
    {
        _mockService.Setup(s => s.ApproveTeamLeaderRegistration(7)).Returns(Task.CompletedTask);

        var result = await _controller.ApproveTeamLeader(7);

        Assert.True(result.IsSuccess);
        Assert.Equal("Phê duyệt trưởng nhóm thành công.", result.Message);
    }

    [Fact]
    public async Task RejectTeamLeader_ValidId_ReturnsSuccess()
    {
        _mockService.Setup(s => s.RejectTeamLeaderRegistration(8)).Returns(Task.CompletedTask);

        var result = await _controller.RejectTeamLeader(8);

        Assert.True(result.IsSuccess);
        Assert.Equal("Từ chối yêu cầu thành công.", result.Message);
    }

    [Fact]
    public async Task RejectVolunteer_ValidId_ReturnsSuccess()
    {
        _mockService.Setup(s => s.RejectVolunteerRegistration(9)).Returns(Task.CompletedTask);

        var result = await _controller.RejectVolunteer(9);

        Assert.True(result.IsSuccess);
        Assert.Equal("Từ chối yêu cầu thành công.", result.Message);
    }

    [Fact]
    public async Task GetParticipatedActivities_ReturnsList()
    {
        var list = new List<VolunteerDTO> { new VolunteerDTO { UserId = "u100" } };
        _mockService.Setup(s => s.GetParticipatedActivitiesByUserId("u100")).ReturnsAsync(list);

        var result = await _controller.GetParticipatedActivities("u100");

        Assert.True(result.IsSuccess);
        Assert.Equal(list, result.Result);
    }

    [Fact]
    public async Task GetApprovedRole_ReturnsRoleText()
    {
        _mockService.Setup(s => s.GetApprovedRoleAsync(1, "u101")).ReturnsAsync("Volunteer");

        var result = await _controller.GetApprovedRole(1, "u101");

        Assert.True(result.IsSuccess);
        Assert.Contains("Volunteer", result.Message);
    }

    [Fact]
    public async Task HasApprovedTeamLeader_ReturnsTrue()
    {
        _mockService.Setup(s => s.HasApprovedTeamLeaderAsync(2)).ReturnsAsync(true);

        var result = await _controller.HasApprovedTeamLeader(2);

        Assert.True(result.IsSuccess);
        Assert.Contains("Đã có trưởng nhóm", result.Message);
    }
    [Fact]
    public async Task GetApprovedVolunteers_ValidUser_ReturnsList()
    {
        var list = new List<VolunteerDTO> { new VolunteerDTO { UserId = "u222" } };
        _mockService.Setup(s => s.GetApprovedVolunteersByUserIdAsync("u222")).ReturnsAsync(list);

        var result = await _controller.GetApprovedVolunteers("u222");

        Assert.True(result.IsSuccess);
        Assert.Equal("Lấy danh sách tình nguyện viên đã duyệt thành công.", result.Message);
        Assert.Equal(list, result.Result);
    }

    [Fact]
    public async Task GetTeamLeaderByEventId_ValidEvent_ReturnsLeader()
    {
        var leader = "u333";
        _mockService.Setup(s => s.GetTeamLeaderByEventId(3)).ReturnsAsync(leader);

        var result = await _controller.GetTeamLeaderByEventId(3);

        Assert.True(result.IsSuccess);
        Assert.Equal(leader, result.Result);
        Assert.Equal("Lấy leaderId thành công", result.Message);
    }

        [Fact]
        public async Task IsVolunteer_Confirmed()
        {
            _mockService.Setup(s => s.IsVolunteer(1, "u1")).ReturnsAsync(true);

            var result = await _controller.IsVolunteer(1, "u1");

            Assert.True(result.IsSuccess);
            Assert.Equal("Bạn đã đăng ký, vui lòng đợi phê duyệt.", result.Message);
        }

        [Fact]
        public async Task IsTeamLeader_NotRegistered_ReturnsFalseNoti()
        {
            _mockService.Setup(s => s.IsTeamLeader(2, "u2")).ReturnsAsync(false);

            var result = await _controller.IsTeamLeader(2, "u2");

            Assert.False(result.IsSuccess);
            Assert.Equal("Chưa đăng ký!", result.Message);
        }

        [Fact]
        public async Task UpdateRegister_ValidDto_ReturnsSuccessully()
        {
            var dto = new VolunteerDTO { UserId = "u3" };
            _mockService.Setup(s => s.UpdateRegister(dto)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateRegister(dto);

            Assert.True(result.IsSuccess);
            Assert.Equal("Cập nhật đăng ký thành công.", result.Message);
        }
    }

}
