using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using UsefulCMS.Web;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;
using UsefulServices.Services.Users;

namespace UsefulCMS.Pages
{
    public class IndexModel : AuthorizedCMSPageModel
    {
        public int UserCount => SuperAdminCount + AdminCount + StandardCount;
        public int SuperAdminCount { get; set; }
        public int AdminCount { get; set; }
        public int StandardCount { get; set; }

        private UserManager<User> _userManager;

        public IndexModel(UserManager<User> userManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            SuperAdminCount = (await _userManager.GetUsersInRoleAsync(RoleType.SuperAdministrator.ToString())).Count;
            AdminCount = (await _userManager.GetUsersInRoleAsync(RoleType.Administrator.ToString())).Count;
            StandardCount = (await _userManager.GetUsersInRoleAsync(RoleType.Standard.ToString())).Count;
        }
    }
}
