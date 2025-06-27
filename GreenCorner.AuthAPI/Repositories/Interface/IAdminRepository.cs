using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;

namespace GreenCorner.AuthAPI.Repositories.Interface
{
	public interface IAdminRepository
	{
		Task<IEnumerable<StaffDTO>> GetAllStaff();
		Task<StaffDTO> GetStaffById(string id);
		Task<string> CreateStaff(StaffDTO staff);
		
		Task<StaffDTO> BlockStaffAccount(string id);
		Task<StaffDTO> UnBlockStaffAccount(string id);

        Task UpdateStaff(StaffDTO item);

    }
}
