using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsefulCMS.Web;
using UsefulDatabase.Model.Users;

namespace UsefulCMS.Pages.Account
{
    public class AccessDeniedModel : CMSPageModel
    {
        private SignInManager<User> _signInManager;

        public AccessDeniedModel(SignInManager<User> signInManager, IMapper mapper) : base(mapper)
        {
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToPage("/Account/Login");

            return Page();
        }
    }
}