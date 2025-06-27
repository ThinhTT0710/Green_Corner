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
		public async Task<StaffDTO> BlockStaffAccount(string id)
		{
			var staff = await _dbcontext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
			if (staff != null)
			{
                staff.LockoutEnd = DateTimeOffset.UtcNow.AddDays(100000);
				_dbcontext.SaveChanges();
                StaffDTO staffDTO = new StaffDTO
                {
					ID = staff.Id,
					FullName = staff.FullName,
					Email = staff.Email,
					Address = staff.Address,
					Avatar = staff.Avatar,
					PhoneNumber = staff.PhoneNumber
				};
				return staffDTO;
			}
			throw new System.Exception("User not found");
		}

		public async Task<string> CreateStaff(StaffDTO staff)
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
				
			};
			try
			{
				var result = await _userManager.CreateAsync(user, staff.Password);
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
					return "";
				}
				else
				{
                    return result.Errors.FirstOrDefault().Description;
                }
			}
			catch (Exception ex)
            {
                throw new Exception("Error creating staff: " + ex.Message);
            }
		}

        public async Task UpdateStaff(StaffDTO staffDTO)
        {
            var staff = await _userManager.FindByEmailAsync(staffDTO.Email);
            if (staff == null)
            {
                throw new Exception("Staff not found.");
            }
			staff.FullName = staffDTO.FullName;
			staff.Address = staffDTO.Address;
            staff.Avatar = staffDTO.Avatar;
            staff.PhoneNumber = staffDTO.PhoneNumber;
            try
            {
                var result = await _userManager.UpdateAsync(staff);
                 await _dbcontext.SaveChangesAsync();
                if (result.Succeeded)
                {
                    if (staffDTO.Role == "EVENTSTAFF")
                    {
                        await _userManager.AddToRoleAsync(staff, "EVENTSTAFF");
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(staff, "SALESTAFF");
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


        public async Task<IEnumerable<StaffDTO>> GetAllStaff()
		{
			var staff = await _dbcontext.Users.ToListAsync();
			List<StaffDTO> userDTOs = new List<StaffDTO>();
			foreach (var user in staff)
			{
				// get role
				var roles = await _userManager.GetRolesAsync(user);
				if (roles.Contains("EVENTSTAFF") || roles.Contains("SALESTAFF"))
				{
					StaffDTO userDTO = new StaffDTO
					{
						ID = user.Id,
						FullName = user.FullName,
						Email = user.Email,
						Address = user.Address,
						Avatar = user.Avatar,
						PhoneNumber = user.PhoneNumber,
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

		public Task<StaffDTO> GetStaffById(string id)
		{
			
				var user = _dbcontext.Users.Where(u => u.Id == id).FirstOrDefault();
				if (user != null)
				{
                StaffDTO userDTO = new StaffDTO
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

		public Task<StaffDTO> UnBlockStaffAccount(string id)
		{
			var user = _dbcontext.Users.Where(u => u.Id == id).FirstOrDefault();
			if (user != null)
			{
				user.LockoutEnd = null;
				_dbcontext.SaveChanges();
                StaffDTO userDTO = new StaffDTO
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
