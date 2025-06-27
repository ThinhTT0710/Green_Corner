using GreenCorner.AuthAPI.Data;
using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Repositories.Interface;
using GreenCorner.AuthAPI.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.AuthAPI.Repositories
{
	public class AdminRepository : IAdminRepository
	{
		private readonly AuthDbContext _dbcontext;
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AdminRepository(AuthDbContext dbcontext, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
		{
			_dbcontext = dbcontext;
			_userManager = userManager;
			_roleManager = roleManager;
			
		}
		public async Task<UserDTO> BlockStaffAccount(string id)
		{
			var user = await _dbcontext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
			if (user != null)
			{
				user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(100000);
				_dbcontext.SaveChanges();
				UserDTO userDTO = new UserDTO
				{
					ID = user.Id,
					FullName = user.FullName,
					Email = user.Email,
					Address = user.Address,
					Avatar = user.Avatar,
					PhoneNumber = user.PhoneNumber
				};
				return userDTO;
			}
			throw new System.Exception("User not found");
		}

		public async Task CreateStaff(StaffDTO staff)
		{
			var user = new User
			{
				UserName = staff.Email,
				Email = staff.Email,
				NormalizedEmail = staff.Email.ToUpper(),
				FullName = staff.FullName,
				Address = staff.Address,
				Avatar = "default.png",
				PhoneNumber = staff.PhoneNumber,
				EmailConfirmed = true
				
			};
			try
			{
				var result = await _userManager.CreateAsync(user);
				if (result.Succeeded)
				{
					if (staff.Role == "EVENTSTAFF")
					{
						await _userManager.AddToRoleAsync(user, "EVENTSTAFF");
					}
					else
					{
						await _userManager.AddToRoleAsync(user, "SALESTAFF");
					}
					return;
				}
				else
				{
					throw new Exception(result.Errors.FirstOrDefault().Description);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		public async Task<IEnumerable<UserDTO>> GetAllStaff()
		{
			var users = await _dbcontext.Users.ToListAsync();
			List<UserDTO> userDTOs = new List<UserDTO>();
			foreach (var user in users)
			{
				// get role
				var roles = await _userManager.GetRolesAsync(user);
				if (roles.Contains("EVENTSTAFF") || roles.Contains("SALESTAFF"))
				{
					UserDTO userDTO = new UserDTO
					{
						ID = user.Id,
						FullName = user.FullName,
						Email = user.Email,
						Address = user.Address,
						Avatar = user.Avatar,
						PhoneNumber = user.PhoneNumber
					};

					if (user.LockoutEnd.HasValue)
					{
						userDTO.IsBan = true;
					}

					userDTOs.Add(userDTO);
				}
			}
			return userDTOs;
		}

		public Task<UserDTO> GetStaffById(string id)
		{
			
				var user = _dbcontext.Users.Where(u => u.Id == id).FirstOrDefault();
				if (user != null)
				{
					UserDTO userDTO = new UserDTO
					{
						ID = user.Id,
						FullName = user.FullName,
						Email = user.Email,
						Address = user.Address,
						Avatar = user.Avatar,
						PhoneNumber = user.PhoneNumber
					};

					if (user.LockoutEnd.HasValue)
					{
						userDTO.IsBan = true;
					}
					return Task.FromResult(userDTO);
				}
				return null;
			
		}

		public Task<UserDTO> UnBlockStaffAccount(string id)
		{
			var user = _dbcontext.Users.Where(u => u.Id == id).FirstOrDefault();
			if (user != null)
			{
				user.LockoutEnd = null;
				_dbcontext.SaveChanges();
				UserDTO userDTO = new UserDTO
				{
					ID = user.Id,
					FullName = user.FullName,
					Email = user.Email,
					Address = user.Address,
					Avatar = user.Avatar,
					PhoneNumber = user.PhoneNumber
				};
				return Task.FromResult(userDTO);
			}
			throw new System.Exception("User not found");
		}

		

	
	}
}
