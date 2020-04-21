using Microsoft.AspNetCore.Identity;
using UsefulDatabase.Model.Roles;
using UsefulDatabase.Model.Users;
using UsefulDatabase.Seeding.Roles;
using UsefulDatabase.Seeding.Users;

namespace UsefulDatabase.Seeding
{
    public static class Seeding
    {
        public static void IdentitySeeding(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            RoleSeeding.SeedRoles(roleManager);
            UserSeeding.SeedUsers(userManager);
        }
    }
}
