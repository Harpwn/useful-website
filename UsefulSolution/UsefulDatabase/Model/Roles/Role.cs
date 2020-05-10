using Microsoft.AspNetCore.Identity;
using UsefulCore.Enums.Roles;

namespace UsefulDatabase.Model.Roles
{
    public class Role : IdentityRole<int>
    {
        public RoleType Type { get; set; }
    }
}
