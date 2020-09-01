using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using UsefulCMS.Models;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;
using UsefulServices.Services.Users.Moderation;

namespace UsefulCMS.Pages.Administration.Users
{
    public class DetailsModel : AuthorizedCMSPageModel
    {
        private UserManager<User> _userManager;
        private IUserBanService _userBanService;

        public User UserDetails { get; set; }

        [BindProperty]
        public int Id { get; set; }

        public bool CanBePromoted { get; set; }
        public bool CanBeDemoted { get; set; }

        public DetailsModel(UserManager<User> userManager, IUserBanService userBanService, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _userBanService = userBanService;
        }

        public async Task OnGetAsync([FromQuery]int id)
        {
            await LoadPage(id);
        }

        public async Task<IActionResult> OnPostUnBanUserAsync([FromForm]int id)
        {
            var result = await _userBanService.UnbanUser(Id);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return await LoadPage(id);
        }

        public async Task<IActionResult> OnPostPromoteUserAsync([FromForm]int id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            var highestRole = GetHighestRole(user);

            if (user != null && highestRole != RoleType.SuperAdministrator)
            {
                var newRole = (RoleType)((int)highestRole + 1);
                await _userManager.AddToRoleAsync(user, newRole.ToString());
            }
                
            return await LoadPage(id);
        }

        public async Task<IActionResult> OnPostDemoteUserAsync([FromForm]int id)
        {
            var user = await _userManager.FindByIdAsync(Id.ToString());
            var highestRole = GetHighestRole(user);

            if (user != null && highestRole != RoleType.Standard)
            {
                if(user.Roles.Count() == 1)
                {
                    var newRole = (RoleType)((int)highestRole - 1);
                    await _userManager.AddToRoleAsync(user, newRole.ToString());
                }
                await _userManager.RemoveFromRoleAsync(user, highestRole.ToString());
            }

            return await LoadPage(id);
        }

        private async Task<IActionResult> LoadPage(int id)
        {
            Id = id;
            var user = await _userManager.FindByIdAsync(Id.ToString());

            if (user == null)
                RedirectToPage("/Administration/Index");

            UserDetails = user;
            CanBeDemoted = GetHighestRole(user) != RoleType.Standard;
            CanBePromoted = GetHighestRole(user) != RoleType.SuperAdministrator;

            return Page();
        }

        private RoleType GetHighestRole(User user) => user.Roles.OrderByDescending(r => r.Role.Type).First().Role.Type;
    }
}