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

        public Task<bool> ChangePassword(ChangePasswordRequestDTO changePasswordRequest)
        {
            var user = _dbContext.Users.Where(u => u.Email == changePasswordRequest.Email).FirstOrDefault();
            if (user == null || user.Id != changePasswordRequest.UserID)
            {
                return Task.FromResult(false);
            }

            var isPasswordValid = _userManager.CheckPasswordAsync(user, changePasswordRequest.OldPassword).GetAwaiter().GetResult();
            if (!isPasswordValid)
            {
                return Task.FromResult(false);
            }

            var result = _userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword, changePasswordRequest.NewPassword).GetAwaiter().GetResult();
            return Task.FromResult(true);
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
                    IsBan = user.LockoutEnabled,
                };
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
    }
}
