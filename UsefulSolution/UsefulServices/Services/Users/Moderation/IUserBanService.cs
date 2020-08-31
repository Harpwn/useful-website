using System;
using System.Threading.Tasks;

namespace UsefulServices.Services.Users.Moderation
{
    public interface IUserBanService
    {
        Task<ServiceActionResult> BanUser(string id, string reason, TimeSpan banDuration);
        Task<ServiceActionResult> UnbanUser(string id);
    }
}
