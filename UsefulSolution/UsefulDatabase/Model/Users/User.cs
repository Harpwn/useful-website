using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using UsefulDatabase.Model.Roles;

namespace UsefulDatabase.Model.Users
{
    public class User : IdentityUser<int>
    {
        public DateTime? BannedUntilDate { get; set; }
        public string BannedReason { get; set; }
        public bool IsBanned => BannedUntilDate.HasValue && BannedUntilDate > DateTime.UtcNow;

        public virtual ICollection<UserRole> Roles { get; set; } = new List<UserRole>();
    }
}
