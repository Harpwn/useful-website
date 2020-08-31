using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UsefulCMS.Models;
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

        private async Task<IActionResult> LoadPage(int id)
        {
            Id = id;
            var user = await _userManager.FindByIdAsync(Id.ToString());

            if (user == null)
                RedirectToPage("/Administration/Index");

            UserDetails = user;

            return Page();
        }
    }
}