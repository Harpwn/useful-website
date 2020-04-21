using Microsoft.AspNetCore.Identity;
using System;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Roles;

namespace UsefulDatabase.Seeding.Roles
{
    public static class RoleSeeding
    {
        public static void SeedRoles(RoleManager<Role> roleManager)
        {
            foreach (var roleEnum in (RoleType[])Enum.GetValues(typeof(RoleType)))
            {
                var roleName = roleEnum.ToString();

                if (!roleManager.RoleExistsAsync(roleName).Result)
                {
                    Role role = new Role();
                    role.Name = roleName;
                    role.Type = roleEnum;
                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }
        }
    }
}
