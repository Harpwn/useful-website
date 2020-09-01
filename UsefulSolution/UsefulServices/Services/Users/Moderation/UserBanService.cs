using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Users;

namespace UsefulServices.Services.Users.Moderation
{
    public class UserBanService : Service, IUserBanService
    {
        private UserManager<User> _userManager;

        public UserBanService(UsefulContext context, UserManager<User> userManager, IMemoryCache cache, IMapper mapper) : base(context, cache, mapper)
        {
            _userManager = userManager;
        }

        public async Task<ServiceActionResult> BanUser(int id, string reason, TimeSpan banDuration)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return new ServiceActionResult("User not found");

            user.BannedUntilDate = DateTime.UtcNow.Add(banDuration);
            user.BannedReason = reason;

            await _userManager.UpdateAsync(user);

            return new ServiceActionResult();
        }

        public async Task<ServiceActionResult> UnbanUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                return new ServiceActionResult("User not found");

            user.BannedUntilDate = null;
            user.BannedReason = string.Empty;

            await _userManager.UpdateAsync(user);

            return new ServiceActionResult();
        }
    }
}
