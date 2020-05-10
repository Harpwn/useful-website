using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsefulCMS.Models;
using UsefulDatabase.Model.Users;

namespace UsefulCMS.Pages.Account
{
    public class LogoutModel : CMSPageModel
    {
        private SignInManager<User> _signInManager;

        public LogoutModel(SignInManager<User> signInManager, IMapper mapper) : base(mapper)
        {
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            _signInManager.SignOutAsync();
            return RedirectToPage("/Index");
        }
    }
}