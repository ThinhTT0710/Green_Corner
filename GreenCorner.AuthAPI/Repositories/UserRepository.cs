using GreenCorner.AuthAPI.Data;
using GreenCorner.AuthAPI.Models;
using GreenCorner.AuthAPI.Models.DTO;
using GreenCorner.AuthAPI.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GreenCorner.AuthAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public UserRepository(AuthDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<bool> ChangePassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Id == changePasswordRequest.UserID);
            if (user == null)
            {
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword);

            if (result.Succeeded)
            {
                // Rất quan trọng: Cập nhật Security Stamp
                await _userManager.UpdateSecurityStampAsync(user);
                return true;
            }

            return false;
        }

        public Task<UserDTO> GetUserById(string userId)
        {
            var user = _dbContext.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user != null)
            {
                UserDTO userDTO = new UserDTO
                {
                    ID = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    Avatar = user.Avatar,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address,
                };

				if (user.LockoutEnd.HasValue)
				{
					userDTO.IsBan = true;
				}
				return Task.FromResult(userDTO);
            }
            return null;
        }

        public async Task<UserDTO> UpdateUser(UserDTO user)
        {
            var userUpdate = await _dbContext.Users.Where(u => u.Id == user.ID).FirstOrDefaultAsync();
            if (userUpdate != null)
            {
                userUpdate.FullName = user.FullName;
                userUpdate.Address = user.Address;
                userUpdate.Avatar = user.Avatar;
                userUpdate.PhoneNumber = user.PhoneNumber;

                _dbContext.Users.Update(userUpdate);
                await _dbContext.SaveChangesAsync();
                return user;
            }
            return null;
        }
        public async Task<bool> CheckPhoneNumber(string phoneNumber, string userId)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Id != userId);
            if (user == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		public async Task<UserDTO> BanUser(string id)
		{
			var user = await _dbContext.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
			if (user != null)
			{
				user.LockoutEnd = DateTimeOffset.UtcNow.AddDays(30);
				_dbContext.SaveChanges();
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

		public async Task<UserDTO> UnBanUser(string id)
		{
			var user = _dbContext.Users.Where(u => u.Id == id).FirstOrDefault();
			if (user != null)
			{
				user.LockoutEnd = null;
				_dbContext.SaveChanges();
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

		public async Task<List<UserDTO>> GetAllUser()
		{
			var users = await _dbContext.Users.ToListAsync();
			List<UserDTO> userDTOs = new List<UserDTO>();
			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				if (roles.Contains("CUSTOMER"))
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
	}
}
