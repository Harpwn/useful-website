using System.Threading.Tasks;

namespace UsefulServices.Services.Users
{
    public interface IUserService
    {
        Task<UserActionResult> GetByIDAsync(string id);
    }
}
