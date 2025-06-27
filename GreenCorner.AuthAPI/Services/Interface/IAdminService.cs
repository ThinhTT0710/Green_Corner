using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.AuthAPI.Services.Interface
{
	public interface IAdminService
	{
		Task<IEnumerable<StaffDTO>> GetAllStaff();
		Task<StaffDTO> GetStaffById(string id);
		Task<string> CreateStaff(StaffDTO staff);
		Task<StaffDTO> BlockStaffAccount(string id);
		Task<StaffDTO> UnBlockStaffAccount(string id);
    Task UpdateStaff(StaffDTO staff);
		Task AddLogStaff(SystemLogDTO log);
		Task<IEnumerable<SystemLogDTO>> GetAllLogs();
	}
}
