using Azure;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.AuthAPI.Controllers
{
	[Route("api/Admin")]
	[ApiController]
	public class AdminAPIController : ControllerBase
	{

		private readonly IAdminService _adminService;
		private readonly ResponseDTO _responseDTO;
		public AdminAPIController(IAdminService adminService)
		{
			_adminService = adminService;
			_responseDTO = new ResponseDTO();
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
		public async Task<ResponseDTO> CreateStaff([FromBody] StaffDTO staff)
		{
			try
			{
				await _adminService.CreateStaff(staff);
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
