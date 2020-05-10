using Microsoft.AspNetCore.Identity;

namespace UsefulDatabase.Model.Roles
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual Role Role { get; set; }
    }
}
