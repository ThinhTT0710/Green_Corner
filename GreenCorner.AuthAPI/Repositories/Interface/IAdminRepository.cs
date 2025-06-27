using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.AuthAPI.Repositories.Interface
{
	public interface IAdminRepository
	{
		Task<IEnumerable<UserDTO>> GetAllStaff();
		Task<UserDTO> GetStaffById(string id);
		Task CreateStaff(StaffDTO staff);
		Task<UserDTO> BlockStaffAccount(string id);
		Task<UserDTO> UnBlockStaffAccount(string id);
		Task<IEnumerable<SystemLog>> GetAllLogs();
		Task AddLogStaff(SystemLog log);
	}
}
