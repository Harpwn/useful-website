using System;
using System.Collections.Generic;
using System.Text;

namespace UsefulServices.Dtos.Users
{
    public class UserRoleSummaryDto
    {
        public int UserCount { get; set; }
        public int SuperAdminCount { get; set; }
        public int AdminCount { get; set; }
        public int StandardCount { get; set; }
    }
}
