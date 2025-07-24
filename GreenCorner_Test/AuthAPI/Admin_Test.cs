using GreenCorner.AuthAPI.Controllers;
using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using GreenCorner.MVC.Controllers;
using GreenCorner.MVC.Models;
using GreenCorner.MVC.Models.Admin;
using GreenCorner.MVC.Services.Interface;
using GreenCorner.MVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResponseDTO = GreenCorner.MVC.Models.ResponseDTO;

namespace GreenCorner_Test.AuthAPI
{
    public class Admin_Test
    {
        private readonly Mock<GreenCorner.AuthAPI.Services.Interface.IAdminService> _mockAdminService = new();
        private readonly Mock<IEmailService> _mockEmailService = new();
        private readonly Mock<UserManager<GreenCorner.AuthAPI.Models.User>> _mockUserManager;
        private readonly AdminAPIController _controller;

        public Admin_Test()
        {
            _mockUserManager = MockUserManager<GreenCorner.AuthAPI.Models.User>();
            _controller = new AdminAPIController(
                _mockAdminService.Object,
                _mockUserManager.Object,
                _mockEmailService.Object
            );
        }

        public static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }
        [Fact]
        public async Task GetStaffs_ReturnsStaffList()
        {
            var expected = new List<GreenCorner.AuthAPI.Models.DTO.StaffDTO> { new GreenCorner.AuthAPI.Models.DTO.StaffDTO { ID = "1", FullName = "Test" } };
            _mockAdminService.Setup(s => s.GetAllStaff()).ReturnsAsync(expected);

            var result = await _controller.GetStaffs();

            Assert.True(result.IsSuccess);
            Assert.Equal(expected, result.Result);
        }
        [Fact]
        public async Task GetStaffById_ValidId_ReturnsStaff()
        {
            var staff = new GreenCorner.AuthAPI.Models.DTO.StaffDTO { ID = "1", FullName = "Tester" };
            _mockAdminService.Setup(x => x.GetStaffById("1")).ReturnsAsync(staff);

            var result = await _controller.GetStaffById("1");

            Assert.True(result.IsSuccess);
            Assert.Equal(staff, result.Result);
        }
        //[Fact]
        //public async Task CreateStaff_ValidRequest_SendsEmailAndReturnsSuccess()
        //{
        //    var staff = new GreenCorner.AuthAPI.Models.DTO.StaffDTO { Email = "test@example.com", Password = "pass" };
        //    var user = new GreenCorner.AuthAPI.Models.User { Id = "1", Email = staff.Email };

        //    _mockAdminService.Setup(s => s.CreateStaff(staff)).ReturnsAsync(string.Empty);
        //    _mockUserManager.Setup(m => m.FindByEmailAsync(staff.Email)).ReturnsAsync(user);
        //    _mockUserManager.Setup(m => m.GenerateEmailConfirmationTokenAsync(user)).ReturnsAsync("token");
        //    _mockEmailService.Setup(e => e.SendEmailAsync(staff.Email, It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(Task.CompletedTask);

        //    var result = await _controller.CreateStaff(staff);

        //    var okResult = Assert.IsType<OkObjectResult>(result);
        //    var response = Assert.IsType<ResponseDTO>(okResult.Value);
        //    Assert.Equal("Account created successfully, please check email.", response.Message);
        //}
        [Fact]
        public async Task UpdateStaff_ValidStaff_ReturnsSuccess()
        {
            var staff = new GreenCorner.AuthAPI.Models.DTO.StaffDTO { ID = "1", FullName = "Updated Name" };
            _mockAdminService.Setup(s => s.UpdateStaff(staff)).Returns(Task.CompletedTask);

            var result = await _controller.UpdateStaff(staff);

            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task BlockStaffAccount_ValidId_ReturnsBlockedUser()
        {
            var staff = new GreenCorner.AuthAPI.Models.DTO.StaffDTO { ID = "1", FullName = "Blocked" };
            _mockAdminService.Setup(s => s.BlockStaffAccount("1")).ReturnsAsync(staff);

            var result = await _controller.BlockStaffAccount("1");

            Assert.True(result.IsSuccess);
            Assert.Equal("Staff has been banned forever", result.Message);
            Assert.Equal(staff, result.Result);
        }
        [Fact]
        public async Task UnBlockStaffAccount_ValidId_ReturnsUnblockedUser()
        {
            var staff = new GreenCorner.AuthAPI.Models.DTO.StaffDTO { ID = "1", FullName = "Unblocked" };
            _mockAdminService.Setup(s => s.UnBlockStaffAccount("1")).ReturnsAsync(staff);

            var result = await _controller.UnBlockStaffAccount("1");

            Assert.True(result.IsSuccess);
            Assert.Equal("Staff has been unban", result.Message);
            Assert.Equal(staff, result.Result);
        }
        [Fact]
        public async Task GetAllLogs_ReturnsLogList()
        {
            var logs = new List<GreenCorner.AuthAPI.Models.DTO.SystemLogDTO> { new GreenCorner.AuthAPI.Models.DTO.SystemLogDTO { Id = 1, ActionType = "LOGIN" } };
            _mockAdminService.Setup(s => s.GetAllLogs()).ReturnsAsync(logs);

            var result = await _controller.GetAllLogs();

            Assert.True(result.IsSuccess);
            Assert.Equal(logs, result.Result);
        }
        [Fact]
        public async Task AddLogStaff_ValidLog_ReturnsSuccess()
        {
            var log = new GreenCorner.AuthAPI.Models.DTO.SystemLogDTO { UserId = "1", ActionType = "CREATE" };
            _mockAdminService.Setup(s => s.AddLogStaff(log)).Returns(Task.CompletedTask);

            var result = await _controller.AddLogStaff(log);

            Assert.True(result.IsSuccess);
        }
    }

}


