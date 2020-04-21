using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Users;
using UsefulServices.Dtos.Users;

namespace UsefulServices.Services.Users
{
    public class UserService : Service, IUserService
    {
        private UserManager<User> _userManager;

        public UserService(UsefulContext context, UserManager<User> userManager, IMemoryCache cache, IMapper mapper) : base(context, cache, mapper)
        {
            _userManager = userManager;
        }

        public async Task<UserActionResult> AuthenticateAsync(AuthUserDto dto)
        {
            if (string.IsNullOrEmpty(dto.UserName) || string.IsNullOrEmpty(dto.Password))
                return new UserActionResult("User & Password is required");

            var user = await _userManager.FindByNameAsync(dto.UserName);
            if (user == null)
                return new UserActionResult("User doesnt exist");

            var passwordIsCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordIsCorrect)
                return new UserActionResult("Incorrect Password");

            // authentication successful
            return new UserActionResult(Mapper.Map<UserDto>(user));
        }

        public async Task<ServiceActionResult> CreateAsync(RegisterUserDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrEmpty(dto.UserName))
                return new ServiceActionResult("Username & Password are required");

            var existingUser = await _userManager.FindByNameAsync(dto.UserName);
            if (existingUser != null)
                return new ServiceActionResult("Username \"" + dto.UserName + "\" is already taken");

            var result = await _userManager.CreateAsync(new User
            {
                UserName = dto.UserName,
            }, dto.Password);

            return new ServiceActionResult(result);
        }

        public async Task<UserActionResult> UpdateAsync(UserDto dto)
        {
            if (string.IsNullOrEmpty(dto.Id) || string.IsNullOrEmpty(dto.UserName))
                return new UserActionResult("ID & Username is required");

            var existingUser = await _userManager.FindByIdAsync(dto.Id);

            if (existingUser == null)
                return new UserActionResult("User not found");

            if (dto.UserName != existingUser.UserName)
            {
                if (Context.Users.Any(x => x.UserName == dto.UserName))
                    return new UserActionResult("Username " + dto.UserName + " is already taken");
            }

            // update user properties
            existingUser.UserName = dto.UserName;
            existingUser.Email = dto.Email;

            await _userManager.UpdateAsync(existingUser);

            return new UserActionResult(Mapper.Map<UserDto>(existingUser));
        }

        public async Task<ServiceActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return new ServiceActionResult("User does not exist");

            var result = await _userManager.DeleteAsync(user);
            return new ServiceActionResult(result);
        }

        public async Task<ServiceActionResult> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var existingUser = await _userManager.FindByIdAsync(dto.Id);

            if (existingUser == null)
                return new ServiceActionResult("User not found");

            var result = await _userManager.ChangePasswordAsync(existingUser, dto.Password, dto.NewPassword);
            return new ServiceActionResult(result);
        }

        public async Task<UserActionResult> GetByIDAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
                return new UserActionResult("User not found");

            return new UserActionResult(Mapper.Map<UserDto>(existingUser));
        }
    }
}
