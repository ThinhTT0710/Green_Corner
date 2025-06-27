using Azure;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GreenCorner.AuthAPI.Models;

namespace GreenCorner.AuthAPI.Controllers
{
	[Route("api/Admin")]
	[ApiController]
	public class AdminAPIController : ControllerBase
	{

		private readonly IAdminService _adminService;
		private readonly ResponseDTO _responseDTO;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        protected ResponseDTO _response;
        public AdminAPIController(IAdminService adminService, UserManager<User> userManager, IEmailService emailService)
        {
            _adminService = adminService;
            _responseDTO = new ResponseDTO();
            _userManager = userManager;
            _emailService = emailService;
			this._response = new ResponseDTO();
        }
        [HttpGet]
		public async Task<ResponseDTO> GetStaffs()
		{
			try
			{
				var staffs = await _adminService.GetAllStaff();
				_responseDTO.Result = staffs;
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}
		[HttpGet("{id}")]
		public async Task<ResponseDTO> GetStaffById(string id)
		{
			try
			{
				var user = await _adminService.GetStaffById(id);
				_responseDTO.Result = user;
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}
		[HttpPost]
		public async Task<IActionResult> CreateStaff([FromBody] StaffDTO staff)
        {
            try
            {
				var response = await _adminService.CreateStaff(staff);
                if (!string.IsNullOrEmpty(response))
                {
                    _response.Message = response;
                    _response.IsSuccess = false;
                    _response.Result = null;
                    return BadRequest(_response);
                }
                var userEntity = await _userManager.FindByEmailAsync(staff.Email);
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(userEntity);
                var encodeToken = Base64UrlEncoder.Encode(token);
                var confirmationLink = $"https://localhost:7000/Auth/ConfirmEmail?userId={userEntity.Id}&token={encodeToken}";

                await _emailService.SendEmailAsync(staff.Email, "Verify Your Email", $"<h1>Welcome to GreenCorner</h1><p>Please confirm your email by <a href='{confirmationLink}'>clicking here</a></p>");
                _response.Message = "Account created successfully, please check email.";
                return Ok(_responseDTO);
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return Ok(_responseDTO);
			}
		}

        [HttpPut("update-staff")]
        public async Task<ResponseDTO> UpdateStaff([FromBody] StaffDTO staff)
        {
            try
            {
				
                await _adminService.UpdateStaff(staff);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }


        [HttpGet("block-staff/{id}")]
		//[Authorize(Roles = "ADMIN,STAFF")]
		public async Task<ResponseDTO> BlockStaffAccount(string id)
		{
			try
			{
				var user = await _adminService.BlockStaffAccount(id);
				if (user == null)
				{
					_responseDTO.Message = "Staff not found";
					_responseDTO.IsSuccess = false;
					return _responseDTO;
				}
				_responseDTO.Message = "Staff has been banned forever";
				_responseDTO.Result = user;
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

		[HttpGet("unblock-staff/{id}")]
		//[Authorize(Roles = "ADMIN,STAFF")]
		public async Task<ResponseDTO> UnBlockStaffAccount(string id)
		{
			try
			{
				var user = await _adminService.UnBlockStaffAccount(id);
				if (user == null)
				{
					_responseDTO.Message = "Staff not found";
					_responseDTO.IsSuccess = false;
					return _responseDTO;
				}
				_responseDTO.Message = "Staff has been unban";
				_responseDTO.Result = user;
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}
	}
}
