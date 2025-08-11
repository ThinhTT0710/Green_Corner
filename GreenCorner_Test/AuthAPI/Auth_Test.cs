using GreenCorner.AuthAPI.Controllers;
using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenCorner_Test.AuthAPI
{
	public class Auth_Test
	{
		private readonly Mock<IAuthService> _mockAuthService;
		private readonly Mock<UserManager<User>> _mockUserManager;
		private readonly Mock<IEmailService> _mockEmailService;
		private readonly AuthAPIController _controller;
		private readonly List<UserDTO> _mockUsers;

		public Auth_Test()
		{
			_mockAuthService = new Mock<IAuthService>();
			_mockUserManager = new Mock<UserManager<User>>(new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);
			_mockEmailService = new Mock<IEmailService>();
			_controller = new AuthAPIController(_mockAuthService.Object, _mockUserManager.Object, _mockEmailService.Object);
			_mockUsers = GetMockUsers();
		}
		private static Mock<UserManager<User>> MockUserManager()
		{
			var store = new Mock<IUserStore<User>>();
			return new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
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
			public async Task Register_ShouldReturnSuccess()
			{
				var request = new RegisterationRequestDTO
				{
					FullName = "Alice",
					Email = "alice@example.com",
					Password = "SecurePassword123",
					RoleName = "User"
				};

				_mockAuthService.Setup(s => s.Register(request)).ReturnsAsync(string.Empty);
				_mockUserManager.Setup(s => s.FindByEmailAsync(request.Email)).ReturnsAsync(new User { Id = "3", Email = request.Email });
				_mockUserManager.Setup(s => s.GenerateEmailConfirmationTokenAsync(It.IsAny<User>())).ReturnsAsync("token123");

				var result = await _controller.Register(request) as OkObjectResult;

				Assert.NotNull(result);
				Assert.Equal(200, result.StatusCode);
			}

			[Fact]
			public async Task Login_ShouldFail_InvalidCredentials()
			{
				var request = new LoginRequestDTO { Email = "wrong@example.com", Password = "wrongpassword" };

				_mockAuthService.Setup(s => s.Login(request)).ReturnsAsync(new LoginResponseDTO { User = null });

				var result = await _controller.Login(request) as BadRequestObjectResult;

				Assert.NotNull(result);
				Assert.Equal("Email hoặc mật khẩu không đúng", ((ResponseDTO)result.Value).Message);
			}

			[Fact]
			public async Task Login_ShouldFail_UnconfirmedEmail()
			{
				var request = new LoginRequestDTO { Email = "john@example.com", Password = "password123" };

				_mockAuthService.Setup(s => s.Login(request)).ReturnsAsync(new LoginResponseDTO { User = new UserDTO { Email = request.Email } });
				_mockUserManager.Setup(s => s.FindByEmailAsync(request.Email)).ReturnsAsync(new User { Email = request.Email });
				_mockUserManager.Setup(s => s.IsEmailConfirmedAsync(It.IsAny<User>())).ReturnsAsync(false);

				var result = await _controller.Login(request) as BadRequestObjectResult;

				Assert.NotNull(result);
				Assert.Equal("Vui lòng xác nhận email của bạn trước khi đăng nhập.", ((ResponseDTO)result.Value).Message);
			}

			[Fact]
			public async Task ForgotPassword_ShouldFail_UserNotFound()
			{
				var request = new ForgotPasswordRequestDTO { UserId = "999", Token = "wrongtoken", Password = "newpass123", ConfirmPassword = "newpass123" };

				_mockUserManager.Setup(s => s.FindByIdAsync(request.UserId)).ReturnsAsync((User)null);

				var result = await _controller.ForgotPassword(request) as NotFoundObjectResult;

				Assert.NotNull(result);
				Assert.Equal("Không tìm thấy người dùng.", ((ResponseDTO)result.Value).Message);
			}
			[Fact]
			public async Task Login_ShouldFail_WhenUserIsNull()
			{
				var request = new LoginRequestDTO { Email = "abc@gmail.com", Password = "password" };
				_mockAuthService.Setup(s => s.Login(request)).ReturnsAsync(new LoginResponseDTO { User = null });

				var result = await _controller.Login(request);

				var badResult = Assert.IsType<BadRequestObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(badResult.Value);
				Assert.False(response.IsSuccess);
				Assert.Equal("Email hoặc mật khẩu không đúng", response.Message);
			}

			[Fact]
			public async Task Login_ShouldFail_WhenEmailNotConfirmed()
			{
				var request = new LoginRequestDTO { Email = "abc@gmail.com", Password = "password" };
				var loginResponse = new LoginResponseDTO
				{
					User = new UserDTO { Email = request.Email }
				};

				var userEntity = new User { Email = request.Email };
				_mockAuthService.Setup(s => s.Login(request)).ReturnsAsync(loginResponse);
				_mockUserManager.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(userEntity);
				_mockUserManager.Setup(m => m.IsEmailConfirmedAsync(userEntity)).ReturnsAsync(false);

				var result = await _controller.Login(request);

				var badResult = Assert.IsType<BadRequestObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(badResult.Value);
				Assert.Equal("Vui lòng xác nhận email của bạn trước khi đăng nhập.", response.Message);
			}

			[Fact]
			public async Task Login_ShouldSucceed_WhenUserAndEmailValid()
			{
				var request = new LoginRequestDTO { Email = "john@gmail.com", Password = "123456" };
				var userEntity = new User { Email = request.Email };
				var loginResponse = new LoginResponseDTO
				{
					User = new UserDTO { Email = request.Email },
					Token = "valid-token"
				};

				_mockAuthService.Setup(s => s.Login(request)).ReturnsAsync(loginResponse);
				_mockUserManager.Setup(m => m.FindByEmailAsync(request.Email)).ReturnsAsync(userEntity);
				_mockUserManager.Setup(m => m.IsEmailConfirmedAsync(userEntity)).ReturnsAsync(true);

				var result = await _controller.Login(request);

				var okResult = Assert.IsType<OkObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(okResult.Value);
				Assert.Equal(loginResponse, response.Result);
				Assert.True(response.IsSuccess);
			}
			[Fact]
			public async Task GoogleLogin_ShouldFail_WhenLoginFails()
			{
				var request = new GoogleLoginRequestDTO { Email = "noone@gmail.com" };
				_mockAuthService.Setup(s => s.LoginWithGoogle(request)).ReturnsAsync(new LoginResponseDTO { User = null });

				var result = await _controller.GoogleLogin(request);

				var badResult = Assert.IsType<BadRequestObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(badResult.Value);
				Assert.Equal("Đăng nhập với Google không thành công. Vui lòng thử lại", response.Message);
				Assert.False(response.IsSuccess);
			}

			[Fact]
			public async Task GoogleLogin_ShouldSucceed_WhenLoginValid()
			{
				var request = new GoogleLoginRequestDTO { Email = "user@gmail.com" };
				var loginResponse = new LoginResponseDTO
				{
					User = new UserDTO { Email = request.Email },
					Token = "google-token"
				};

				_mockAuthService.Setup(s => s.LoginWithGoogle(request)).ReturnsAsync(loginResponse);

				var result = await _controller.GoogleLogin(request);

				var okResult = Assert.IsType<OkObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(okResult.Value);
				Assert.True(response.IsSuccess);
				Assert.Equal(loginResponse, response.Result);
			}

			[Fact]
			public async Task FacebookLogin_ShouldFail_WhenUserIsNull()
			{
				var request = new FacebookLoginRequestDTO { Email = "fail@gmail.com" };
				_mockAuthService.Setup(s => s.LoginWithFacebook(request)).ReturnsAsync(new LoginResponseDTO { User = null });

				var result = await _controller.FacebookLogin(request);

				var badResult = Assert.IsType<BadRequestObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(badResult.Value);
				Assert.Equal("Đăng nhập với Facebook không thành công. Vui lòng thử lại", response.Message);
			}
        [Fact]
        public async Task FacebookLogin_ShouldFail_WhenUserIsInvalid()
        {
            var request = new FacebookLoginRequestDTO { Email = "fail@gmail.com" };
            _mockAuthService.Setup(s => s.LoginWithFacebook(request)).ReturnsAsync(new LoginResponseDTO { User = null });

            var result = await _controller.FacebookLogin(request);

            var badResult = Assert.IsType<BadRequestObjectResult>(result);
            var response = Assert.IsType<ResponseDTO>(badResult.Value);
            Assert.Equal("Đăng nhập với Facebook không thành công. Vui lòng thử lại", response.Message);
        }

        [Fact]
			public async Task FacebookLogin_ShouldSucceed()
			{
				var request = new FacebookLoginRequestDTO { Email = "valid@gmail.com" };
				var loginResponse = new LoginResponseDTO
				{
					User = new UserDTO { Email = request.Email },
					Token = "fb-token"
				};

				_mockAuthService.Setup(s => s.LoginWithFacebook(request)).ReturnsAsync(loginResponse);

				var result = await _controller.FacebookLogin(request);

				var okResult = Assert.IsType<OkObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(okResult.Value);
				Assert.True(response.IsSuccess);
				Assert.Equal(loginResponse, response.Result);
			}
        [Fact]
        public async Task FacebookLogin_ShouldSucceedWithValidUser()
        {
            var request = new FacebookLoginRequestDTO { Email = "valid@gmail.com" };
            var loginResponse = new LoginResponseDTO
            {
                User = new UserDTO { Email = request.Email },
                Token = "fb-token"
            };

            _mockAuthService.Setup(s => s.LoginWithFacebook(request)).ReturnsAsync(loginResponse);

            var result = await _controller.FacebookLogin(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO>(okResult.Value);
            Assert.True(response.IsSuccess);
            Assert.Equal(loginResponse, response.Result);
        }
        [Fact]
			public async Task ConfirmEmail_Valid_ReturnsSuccess()
			{
				var user = new User { Id = "1" };
				_mockUserManager.Setup(x => x.FindByIdAsync(user.Id)).ReturnsAsync(user);
				_mockUserManager.Setup(x => x.ConfirmEmailAsync(user, It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

				var result = await _controller.ConfirmEmail(user.Id, Base64UrlEncoder.Encode("token"));

				var ok = Assert.IsType<OkObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(ok.Value);
				Assert.Equal("Email đã được xác nhận thành công.", response.Message);
			}
			[Fact]
			public async Task ResendConfirmEmail_ValidEmail_SendsEmail()
			{
				var user = new User { Id = "1", Email = "test@a.com" };
				_mockUserManager.Setup(x => x.FindByEmailAsync(user.Email)).ReturnsAsync(user);
				_mockUserManager.Setup(x => x.GenerateEmailConfirmationTokenAsync(user)).ReturnsAsync("token");
				_mockEmailService.Setup(x => x.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

				var result = await _controller.ResendConfirmEmail(user.Email);
				var ok = Assert.IsType<OkObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(ok.Value);
				Assert.Equal("Email đã được gửi thành công, vui lòng kiểm tra email.", response.Message);
			}
			[Fact]
			public async Task AssignRole_ValidInput_ReturnsOk()
			{
				var request = new RegisterationRequestDTO
				{
					Email = "test@example.com",
					RoleName = "USER"
				};

				_mockAuthService
					.Setup(x => x.AssignRole(request.Email, request.RoleName.ToUpper()))
					.ReturnsAsync(true);

				var result = await _controller.AssignRole(request);

				var ok = Assert.IsType<OkObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(ok.Value);
				Assert.True(response.IsSuccess);
			}
			[Fact]
			public async Task GetSecurityStamp_UserFound_ReturnsOkWithStamp()
			{
				var user = new User
				{
					Id = "123",
					SecurityStamp = "xyzstamp"
				};

				_mockUserManager
					.Setup(x => x.FindByIdAsync(user.Id))
					.ReturnsAsync(user);

				var result = await _controller.GetSecurityStamp(user.Id);

				var ok = Assert.IsType<OkObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(ok.Value);
				Assert.True(response.IsSuccess);
				Assert.Equal("xyzstamp", response.Result);
			}
			[Fact]
			public async Task GetSecurityStamp_UserNotFound_ReturnsNotFound()
			{
				_mockUserManager
					.Setup(x => x.FindByIdAsync("invalid"))
					.ReturnsAsync((User)null);

				var result = await _controller.GetSecurityStamp("invalid");

				var notFound = Assert.IsType<NotFoundObjectResult>(result);
				var response = Assert.IsType<ResponseDTO>(notFound.Value);
				Assert.False(response.IsSuccess);
				Assert.Equal("Không tìm thấy người dùng.", response.Message);
			}

		}
	}

