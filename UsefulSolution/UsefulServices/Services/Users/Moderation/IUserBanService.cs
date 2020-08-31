using System;
using System.Threading.Tasks;

namespace UsefulServices.Services.Users.Moderation
{
    public interface IUserBanService
    {
        Task<ServiceActionResult> BanUser(int id, string reason, TimeSpan banDuration);
        Task<ServiceActionResult> UnbanUser(int id);
    }
}
