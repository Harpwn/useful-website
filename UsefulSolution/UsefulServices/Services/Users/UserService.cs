using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsefulCore.Enums.Roles;
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

        public async Task<UserActionResult> GetByIDAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
                return new UserActionResult("User not found");

            return new UserActionResult(Mapper.Map<UserDto>(existingUser));
        }
    }
}
