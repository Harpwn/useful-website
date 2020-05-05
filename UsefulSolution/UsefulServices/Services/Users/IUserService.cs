using System.Threading.Tasks;
using UsefulServices.Dtos.Users;

namespace UsefulServices.Services.Users
{
    public interface IUserService
    {
        Task<UserActionResult> GetByIDAsync(string id);
    }
}
