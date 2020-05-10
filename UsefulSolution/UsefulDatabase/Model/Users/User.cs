using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using UsefulDatabase.Model.Roles;

namespace UsefulDatabase.Model.Users
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
