using Microsoft.AspNetCore.Identity;
using UsefulCore.Models.Users;

namespace UsefulServices.Services.Users
{
    public class UserActionResult : ServiceActionResult
    {
        public UserActionResult(UserSummary user, IdentityResult result) : base(result)
        {
            User = user;
        }

        public UserActionResult(string error) : base(error) { }

        public UserActionResult(UserSummary user)
        {
            User = user;
        }

        public UserSummary User { get; set; }
    }
}
