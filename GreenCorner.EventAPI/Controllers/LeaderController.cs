using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EventAPI.Controllers
{
	[Route("api/Leader")]
	[ApiController]
	public class LeaderController : Controller
	{
		private readonly ILeaderService _leaderService;
		private readonly ResponseDTO _responseDTO;
		public LeaderController(ILeaderService leaderService)
		{
			_leaderService = leaderService;
			_responseDTO = new ResponseDTO();
		}
		[HttpGet("{id}")]
		public async Task<ResponseDTO> ViewVolunteerList(int id)
		{
			try
			{
				var volunteerList = await _leaderService.ViewVolunteerList(id);
				_responseDTO.Result = volunteerList;
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = ex.Message;
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

        [HttpGet("{userID},{eventId},{check}")]
        public async Task<ResponseDTO> AttendanceCheck(string userID, int eventId, bool check)
        {
            try
            {
                 await _leaderService.AttendanceCheck(userID, eventId, check);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = ex.Message;
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpDelete("{userID},{eventId}")]
        public async Task<ResponseDTO> KickVolunteer(string userID, int eventId)
        {
            try
            {
                await _leaderService.KickVolunteer(userID, eventId);
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
