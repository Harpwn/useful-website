using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsefulCore.Models.Users;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Users;

namespace UsefulServices.Services.Users
{
    public class UserService : Service, IUserService
    {
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public UserService(UsefulContext context, UserManager<User> userManager, IMapper mapper) : base(context)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserActionResult> GetByIDAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
                return new UserActionResult("User not found");

            return new UserActionResult(_mapper.Map<UserSummary>(existingUser));
        }
    }
}
