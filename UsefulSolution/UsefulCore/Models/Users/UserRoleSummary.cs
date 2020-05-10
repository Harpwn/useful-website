using System.Collections.Generic;
using UsefulCore.Enums.Roles;

namespace UsefulCore.Models.Users
{
    public class UserRoleSummary : UserSummary
    {
        public IEnumerable<RoleType> Roles { get; set; }
    }
}
