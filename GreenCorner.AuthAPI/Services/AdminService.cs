using AutoMapper;
using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Repositories.Interface;
using GreenCorner.AuthAPI.Services.Interface;

namespace GreenCorner.AuthAPI.Services
{
	public class AdminService : IAdminService
	{
		private readonly IAdminRepository _adminRepository;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;

		public AdminService(IAdminRepository adminRepository, IMapper mapper, IUserService userService)
		{
			_adminRepository = adminRepository;
			_mapper = mapper;
			_userService = userService;
		}
		public async Task<IEnumerable<UserDTO>> GetAllStaff()
		{
			var users = await _adminRepository.GetAllStaff();
			return users;
		}
		public async Task<UserDTO> GetStaffById(String id)
		{
			var users = await _adminRepository.GetStaffById(id);
			return users;
		}
		public async Task CreateStaff(StaffDTO staff)
		{
			await _adminRepository.CreateStaff(staff);
		}

		public async Task<UserDTO> BlockStaffAccount(string id)
		{
			var users = await _adminRepository.BlockStaffAccount(id);
			return users;
		}

		public async Task<UserDTO> UnBlockStaffAccount(string id)
		{
			var users = await _adminRepository.UnBlockStaffAccount(id);
			return users;
		}

		public async Task<IEnumerable<SystemLogDTO>> GetAllLogs()
		{
			var logs = await _adminRepository.GetAllLogs();
			return _mapper.Map<List<SystemLogDTO>>(logs);
		}

		public async Task AddLogStaff(SystemLogDTO log)
		{
			SystemLog staffLog = _mapper.Map<SystemLog>(log);
			await _adminRepository.AddLogStaff(staffLog);
		}
	}
}
