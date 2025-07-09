//using GreenCorner.AuthAPI.Controllers;
//using GreenCorner.AuthAPI.Models.DTO;
//using GreenCorner.AuthAPI.Services.Interface;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GreenCorner_Test.AuthAPI
//{
//	public class Admin_Test
//	{
//		private readonly Mock<IAdminService> _mockService;
//		private readonly AdminAPIController _controller;
//		private readonly List<StaffDTO> _mockUsers;

//		public Admin_Test()
//		{
//			_mockService = new Mock<IAdminService>();
//			_controller = new AdminAPIController(_mockService.Object);
//			_mockUsers = GetMockUsers();
//		}

//		private static List<StaffDTO> GetMockUsers()
//		{
//			return new List<StaffDTO>
//		{
//			new StaffDTO
//            {
//				ID = "1",
//				FullName = "Alice Nguyen",
//				Email = "alice@example.com",
//				Address = "123 Can Tho",
//				Avatar = "https://example.com/avatar1.png",
//				PhoneNumber = "0901234567",
//				IsBan = false
//			},
//			new StaffDTO
//            {
//				ID = "2",
//				FullName = "Bob Tran",
//				Email = "bob@example.com",
//				Address = "456 Sai Gon",
//				Avatar = "https://example.com/avatar2.png",
//				PhoneNumber = "0987654321",
//				IsBan = false
//			}
//		};
//		}

//		[Fact]
//		public async Task GetStaffs_ShouldReturnAllStaffs()
//		{
//			_mockService.Setup(s => s.GetAllStaff()).ReturnsAsync(_mockUsers);

//			var result = await _controller.GetStaffs();

//			Assert.True(result.IsSuccess);
//			Assert.Equal(2, ((List<StaffDTO>)result.Result).Count);
//		}

//		[Fact]
//		public async Task GetStaffById_ShouldReturnStaff()
//		{
//			var userId = "1";
//			_mockService.Setup(s => s.GetStaffById(userId)).ReturnsAsync(_mockUsers.First());

//			var result = await _controller.GetStaffById(userId);

//			Assert.True(result.IsSuccess);
//			Assert.Equal("Alice Nguyen", ((UserDTO)result.Result).FullName);
//		}

//		[Fact]
//		public async Task BlockStaffAccount_ShouldFail_WhenUserNotFound()
//		{
//			_mockService.Setup(s => s.BlockStaffAccount("999")).ReturnsAsync((UserDTO)null);

//			var result = await _controller.BlockStaffAccount("999");

//			Assert.False(result.IsSuccess);
//			Assert.Equal("Staff not found", result.Message);
//		}

//		[Fact]
//		public async Task BlockStaffAccount_ShouldSuccess_WhenUserFound()
//		{
//			var user = _mockUsers.First();
//			_mockService.Setup(s => s.BlockStaffAccount(user.ID)).ReturnsAsync(user);

//			var result = await _controller.BlockStaffAccount(user.ID);

//			Assert.True(result.IsSuccess);
//			Assert.Equal("Staff has been banned forever", result.Message);
//		}

//		[Fact]
//		public async Task UnBanUser_ShouldFail_WhenUserNotFound()
//		{
//			_mockService.Setup(s => s.UnBlockStaffAccount("888")).ReturnsAsync((UserDTO)null);

//			var result = await _controller.UnBlockStaffAccount("888");

//			Assert.False(result.IsSuccess);
//			Assert.Equal("Staff not found", result.Message);
//		}

//		[Fact]
//		public async Task UnBanUser_ShouldSucceed()
//		{
//			var user = _mockUsers.Last();
//			_mockService.Setup(s => s.UnBlockStaffAccount(user.ID)).ReturnsAsync(user);

//			var result = await _controller.UnBlockStaffAccount(user.ID);

//			Assert.True(result.IsSuccess);
//			Assert.Equal("Staff has been unban", result.Message);
//		}
//		[Fact]
//		public async Task CreateStaff_ShouldSucceed()
//		{
//			var newUser = new StaffDTO
//			{
//				ID = "3",
//				FullName = "Charlie Pham",
//				Email = "charlie@example.com",
//				Address = "789 Ha Noi",
//				Avatar = "https://example.com/avatar3.png",
//				PhoneNumber = "0911222333",
//				IsBan = false
//			};

//			_mockService.Setup(s => s.CreateStaff(newUser)).Returns(Task.CompletedTask);

//			var result = await _controller.CreateStaff(newUser);

//			Assert.True(result.IsSuccess);
//		}

//		[Fact]
//		public async Task CreateStaff_ShouldFail_OnException()
//		{
//			var invalidUser = new StaffDTO
//            {
//				ID = "4",
//				FullName = "Invalid User",
//				Email = "invalid@example.com"
//			};

//			_mockService.Setup(s => s.CreateStaff(invalidUser)).ThrowsAsync(new Exception("Failed to create staff"));

//			var result = await _controller.CreateStaff(invalidUser);

//			Assert.False(result.IsSuccess);
//			Assert.Equal("Failed to create staff", result.Message);
//		}
//	}

//}
