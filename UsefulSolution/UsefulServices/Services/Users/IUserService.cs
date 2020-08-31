using System.Collections.Generic;
using System.Threading.Tasks;
using UsefulCore.Models.Users;

namespace UsefulServices.Services.Users
{
    public interface IUserService
    {
        Task<UserActionResult> GetByIDAsync(string id);
        Task<IEnumerable<UserRoleSummary>> GetUserRoleSummaryAsync();
    }
}
