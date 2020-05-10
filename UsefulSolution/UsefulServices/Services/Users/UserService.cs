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

        public UserService(UsefulContext context, UserManager<User> userManager, IMemoryCache cache, IMapper mapper) : base(context, cache, mapper)
        {
            _userManager = userManager;
        }

        public async Task<UserActionResult> GetByIDAsync(string id)
        {
            var existingUser = await _userManager.FindByIdAsync(id);

            if (existingUser == null)
                return new UserActionResult("User not found");

            return new UserActionResult(Mapper.Map<UserSummary>(existingUser));
        }

        public Task<IEnumerable<UserRoleSummary>> GetUserRoleSummaryAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
