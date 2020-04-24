using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UsefulCMS.Web;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;

namespace UsefulCMS.Pages
{
    public class IndexModel : CMSPageModel
    {
        private UserManager<User> _userManager;

        public RoleType Role { get; set; }

        public IndexModel(IMapper mapper, UserManager<User> userManager) : base(mapper)
        {
            _userManager = userManager;
        }

        public async Task OnGetAsync()
        {
            //var user = await _userManager.GetUserAsync(HttpContext.User);
            //Role = await _userManager.IsInRoleAsync(user, RoleType.SuperAdministrator.ToString()) ? RoleType.SuperAdministrator : RoleType.Administrator;
        }
    }
}
