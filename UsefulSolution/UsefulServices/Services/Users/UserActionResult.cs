using Microsoft.AspNetCore.Identity;
using UsefulServices.Dtos.Users;

namespace UsefulServices.Services.Users
{
    public class UserActionResult : ServiceActionResult
    {
        public UserActionResult(UserDto user, IdentityResult result) : base(result)
        {
            User = user;
        }

        public UserActionResult(string error) : base(error) { }

        public UserActionResult(UserDto user)
        {
            User = user;
        }

        public UserDto User { get; set; }
    }
}
