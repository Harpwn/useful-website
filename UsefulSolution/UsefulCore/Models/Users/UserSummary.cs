using System;

namespace UsefulCore.Models.Users
{
    public class UserSummary
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsBanned { get; set; }
        public string BannedReason { get; set; }
        public DateTime? BannedUntilDate { get; set; }
    }
}
