using GreenCorner.AuthAPI.Controllers;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.AuthAPI
{
	public class User_Test
	{
		private readonly Mock<IUserService> _mockService;
		private readonly UserAPIController _controller;
		private readonly List<UserDTO> _mockUsers;

		public User_Test()
		{
			_mockService = new Mock<IUserService>();
			_controller = new UserAPIController(_mockService.Object);
			_mockUsers = GetMockUsers();
		}

		private static List<UserDTO> GetMockUsers()
		{
			return new List<UserDTO>
		{
			new UserDTO
			{
				ID = "1",
				FullName = "John Doe",
				Email = "john@example.com",
				Address = "123 Street",
				Avatar = "https://example.com/avatar.jpg",
				PhoneNumber = "1234567890",
				IsBan = false
			},
			new UserDTO
			{
				ID = "2",
				FullName = "Jane Doe",
				Email = "jane@example.com",
				Address = "456 Street",
				Avatar = "https://example.com/avatar2.jpg",
				PhoneNumber = "0987654321",
				IsBan = false
			}
		};
		}

		[Fact]
		public async Task GetUserByUserID_ShouldReturnUser()
		{
			_mockService.Setup(s => s.GetUserById("1")).ReturnsAsync(_mockUsers.Find(u => u.ID == "1"));

			var result = await _controller.GetUserByUserID("1");

			Assert.NotNull(result.Result);
		}

		[Fact]
		public async Task GetUserByUserID_ShouldFail_UserNotFound()
		{
			_mockService.Setup(s => s.GetUserById("999")).ReturnsAsync((UserDTO)null);

			var result = await _controller.GetUserByUserID("999");

			Assert.False(result.IsSuccess);
			Assert.Equal("User not found", result.Message);
		}

		[Fact]
		public async Task UpdateProfile_ShouldReturnSuccess()
		{
			var userDTO = _mockUsers.First();
			_mockService.Setup(s => s.CheckPhoneNumber(userDTO.PhoneNumber, userDTO.ID)).ReturnsAsync(true);
			_mockService.Setup(s => s.UpdateUser(userDTO)).ReturnsAsync(userDTO);

			var result = await _controller.UpdateProfie(userDTO);

			Assert.NotNull(result.Result);
			Assert.True(result.IsSuccess);
		}

		[Fact]
		public async Task UpdateProfile_ShouldFail_PhoneNumberExists()
		{
			var userDTO = _mockUsers.First();
			_mockService.Setup(s => s.CheckPhoneNumber(userDTO.PhoneNumber, userDTO.ID)).ReturnsAsync(false);

			var result = await _controller.UpdateProfie(userDTO);

			Assert.False(result.IsSuccess);
			Assert.Equal("Phone number already exists", result.Message);
		}

		[Fact]
		public async Task ChangePassword_ShouldReturnSuccess()
		{
			var request = new ChangePasswordRequestDTO
			{
				Email = "john@example.com",
				UserID = "1",
				OldPassword = "oldpass123",
				NewPassword = "newpass123"
			};

			_mockService.Setup(s => s.ChangePassword(request)).ReturnsAsync(true);

			var result = await _controller.ChangePassword(request) as OkObjectResult;

			Assert.NotNull(result);
			Assert.True(((ResponseDTO)result.Value).IsSuccess);
		}

		[Fact]
		public async Task ChangePassword_ShouldFail()
		{
			var request = new ChangePasswordRequestDTO
			{
				Email = "john@example.com",
				UserID = "1",
				OldPassword = "wrongpass",
				NewPassword = "newpass123"
			};

			_mockService.Setup(s => s.ChangePassword(request)).ReturnsAsync(false);

			var result = await _controller.ChangePassword(request) as BadRequestObjectResult;

			Assert.NotNull(result);
			Assert.False(((ResponseDTO)result.Value).IsSuccess);
			Assert.Equal("Change password failed. Please try again", ((ResponseDTO)result.Value).Message);
		}
	}
}
