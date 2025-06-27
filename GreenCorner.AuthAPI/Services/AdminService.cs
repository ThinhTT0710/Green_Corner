using AutoMapper;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Repositories.Interface;
using GreenCorner.AuthAPI.Services.Interface;

namespace GreenCorner.AuthAPI.Services
{
	public class AdminService : IAdminService
	{
		private readonly IAdminRepository _adminRepository;

		public AdminService(IAdminRepository adminRepository)
		{
			_adminRepository = adminRepository;
		}
		public async Task<IEnumerable<StaffDTO>> GetAllStaff()
		{
			var users = await _adminRepository.GetAllStaff();
			return users;
		}
		public async Task<StaffDTO> GetStaffById(String id)
		{
			var users = await _adminRepository.GetStaffById(id);
			return users;
		}
		public async Task<string> CreateStaff(StaffDTO staff)
		{
			return await _adminRepository.CreateStaff(staff);
		}
        public async Task UpdateStaff(StaffDTO staff)
        {
            await _adminRepository.UpdateStaff(staff);
        }

        public async Task<StaffDTO> BlockStaffAccount(string id)
		{
			var users = await _adminRepository.BlockStaffAccount(id);
			return users;
		}

		public async Task<StaffDTO> UnBlockStaffAccount(string id)
		{
			var users = await _adminRepository.UnBlockStaffAccount(id);
			return users;
		}
	}
}
