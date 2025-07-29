
using GreenCorner.EventAPI.Models.DTO;
using GreenCorner.EventAPI.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenCorner.EventAPI.Controllers
{
    [Route("api/trashevent")]
    [ApiController]
    public class TrashEventController : ControllerBase
    {
        private readonly ITrashEventService _trashEventService;
        private readonly ResponseDTO _responseDTO;

        public TrashEventController(ITrashEventService trashEventService)
        {
            _trashEventService = trashEventService;
            this._responseDTO = new ResponseDTO();
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN,EVENTSTAFF")]
        public async Task<ResponseDTO> GetTrashEvents()
        {
            try
            {
                var trashEvents = await _trashEventService.GetAllTrashEvent();
                _responseDTO.Result = trashEvents;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy điểm rác thất bại! ";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost("uploadImage")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File không hợp lệ");

            // Tạo thư mục nếu chưa có
            var folderPath = Path.Combine("..", "GreenCorner.MVC", "wwwroot", "imgs", "reporttrash");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Tạo đường dẫn file đích
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            // Ghi file
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Trả lại đường dẫn truy cập file
            var fileUrl = $"{Request.Scheme}://{Request.Host}/imgs/reporttrash/{fileName}";
            return Ok(new { fileName = fileName, url = fileUrl });
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN,EVENTSTAFF")]
        public async Task<ResponseDTO> GetTrashEventById(int id)
        {
            try
            {
                var trashEvents = await _trashEventService.GetByTrashEventId(id);
                _responseDTO.Result = trashEvents;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy thông tin điểm rác thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPost]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> CreateTrashEvent([FromBody] TrashEventDTO trashEventDTO)
        {
            try
            {
                await _trashEventService.AddTrashEvent(trashEventDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Tạo báo cáo điểm rác thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> UpdateTrashEvent([FromBody] TrashEventDTO trashEventDTO)
        {
            try
            {
                await _trashEventService.UpdateTrashEvent(trashEventDTO);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Cập nhật thông tin điểm rác thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,EVENTSTAFF")]
        public async Task<ResponseDTO> DeleteTrashEvent(int id)
        {
            try
            {
                await _trashEventService.DeleteTrashEvent(id);
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Xóa điểm rác thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "ADMIN,EVENTSTAFF")]
        public async Task<ResponseDTO> ApproveTrashEvent(int id)
        {
            try
            {
                await _trashEventService.ApproveTrashEvent(id);
                _responseDTO.Message = "Xác nhận sự kiện thành công.";
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Xác nhận sự kiện thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

		[HttpPut("reject/{id}")]
        [Authorize(Roles = "ADMIN,EVENTSTAFF")]
        public async Task<ResponseDTO> RejectTrashEvent(int id)
		{
			try
			{
				await _trashEventService.RejectTrashEvent(id);
				_responseDTO.Message = "Từ chối sự kiện thành công.";
				return _responseDTO;
			}
			catch (Exception ex)
			{
				_responseDTO.Message = "Từ chối sự kiện thất bại!";
				_responseDTO.IsSuccess = false;
				return _responseDTO;
			}
		}

        [HttpGet("get-by-user/{userId}")]
        [Authorize(Roles = "CUSTOMER")]
        public async Task<ResponseDTO> GetUserReport(string userId)
        {
            try
            {
                var trashEvents = await _trashEventService.GetByUserId(userId);
                _responseDTO.Result = trashEvents;
                return _responseDTO;
            }
            catch (Exception ex)
            {
                _responseDTO.Message = "Lấy danh sách báo cáo thất bại!";
                _responseDTO.IsSuccess = false;
                return _responseDTO;
            }
        }

        [HttpGet("monthly-analytics")]
        public async Task<ResponseDTO> GetMonthlyAnalytics()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                var analytics = await _trashEventService.GetMonthlyEventAnalytics(currentYear);
                _responseDTO.Result = analytics;
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
