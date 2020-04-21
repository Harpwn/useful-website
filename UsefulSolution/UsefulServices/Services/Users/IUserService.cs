using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsefulServices.Dtos.Users;

namespace UsefulServices.Services.Users
{
    public interface IUserService
    {
        Task<UserActionResult> AuthenticateAsync(AuthUserDto dto);
        Task<ServiceActionResult> CreateAsync(RegisterUserDto dto);
        Task<UserActionResult> GetByIDAsync(string id);
        Task<UserActionResult> UpdateAsync(UserDto dto);
        Task<ServiceActionResult> ChangePasswordAsync(ChangePasswordDto dto);
        Task<ServiceActionResult> DeleteAsync(string id);
    }
}
