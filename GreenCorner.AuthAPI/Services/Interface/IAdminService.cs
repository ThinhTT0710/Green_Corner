using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.AuthAPI.Services.Interface
{
	public interface IAdminService
	{
		Task<IEnumerable<UserDTO>> GetAllStaff();
		Task<UserDTO> GetStaffById(string id);
		Task CreateStaff(StaffDTO staff);
		Task<UserDTO> BlockStaffAccount(string id);
		Task<UserDTO> UnBlockStaffAccount(string id);
		Task AddLogStaff(SystemLogDTO log);
		Task<IEnumerable<SystemLogDTO>> GetAllLogs();
	}
}
