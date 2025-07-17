using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.AuthAPI.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserAPIController : ControllerBase
    {
        private readonly IUserService _userService;
        protected ResponseDTO _response;

        public UserAPIController(IUserService userService)
        {
            _userService = userService;
            this._response = new ResponseDTO();
        }

        [HttpGet("{id}")]
        public async Task<ResponseDTO> GetUserByUserID(string id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                _response.Message = "User not found";
                _response.IsSuccess = false;
                return _response;
            }
            _response.Result = user;
            return _response;
        }
        [HttpPut]
        public async Task<ResponseDTO> UpdateProfie([FromBody] UserDTO userDTO) 
        {
            var checkPhoneNumber =  await _userService.CheckPhoneNumber(userDTO.PhoneNumber, userDTO.ID);
            if (!checkPhoneNumber)
            {
                _response.Message = "Phone number already exists";
                _response.IsSuccess = false;
                return _response;
            }
            var response = await _userService.UpdateUser(userDTO);
            if (response == null)
            {
                _response.Message = "Update profile failed. Please try again";
                _response.IsSuccess = false;
                return _response;
            }
            _response.Result = response;
            return _response;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDTO changePasswordRequest)
        {
            var response = await _userService.ChangePassword(changePasswordRequest);
            if (!response)
            {
                _response.Message = "Change password failed. Please try again";
                _response.IsSuccess = false;
                return BadRequest(_response);
            }
            return Ok(_response);
        }

		[HttpGet("get-all-user")]
		public async Task<ResponseDTO> GetAllUser()
		{
			var users = await _userService.GetAllUser();
			if (users == null)
			{
				_response.Message = "No user found";
				_response.IsSuccess = false;
				return _response;
			}
			_response.Result = users;
			return _response;
		}

		[HttpGet("ban-user/{id}")]
		public async Task<ResponseDTO> BanUser(string id)
		{
			try
			{
				var user = await _userService.BanUser(id);
				if (user == null)
				{
					_response.Message = "User not found";
					_response.IsSuccess = false;
					return _response;
				}
				_response.Message = "User has been locked for 30 days";
				_response.Result = user;
				return _response;
			}
			catch (Exception ex)
			{
				_response.Message = ex.Message;
				_response.IsSuccess = false;
				return _response;
			}
		}

		[HttpGet("unban-user/{id}")]
		public async Task<ResponseDTO> UnBanUser(string id)
		{
			try
			{
				var user = await _userService.UnBanUser(id);
				if (user == null)
				{
					_response.Message = "User not found";
					_response.IsSuccess = false;
					return _response;
				}
				_response.Message = "User has been unlocked";
				_response.Result = user;
				return _response;
			}
			catch (Exception ex)
			{
				_response.Message = ex.Message;
				_response.IsSuccess = false;
				return _response;
			}
		}
	}
}
