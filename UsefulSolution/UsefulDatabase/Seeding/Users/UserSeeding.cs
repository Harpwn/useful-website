using Microsoft.AspNetCore.Identity;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;

namespace UsefulDatabase.Seeding.Users
{
    public static class UserSeeding
    {
        public static void SeedUsers(UserManager<User> userManager)
        {
            if (userManager.FindByNameAsync("SuperAdmin").Result == null)
            {
                User user = new User();
                user.UserName = "SuperAdmin";

                IdentityResult result = userManager.CreateAsync(user, "ChangeMe!1234").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, RoleType.SuperAdministrator.ToString()).Wait();
                }
            }
        }
    }
}
