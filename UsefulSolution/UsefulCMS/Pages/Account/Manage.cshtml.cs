using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UsefulCMS.Web;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;

namespace UsefulCMS.Pages.Account
{
    public class ManageModel : AuthorizedCMSPageModel
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        [BindProperty]
        [Required]
        [MinLength(4)]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        public bool CanDelete => Username != "SuperAdmin";

        public ManageModel(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            Username = user.UserName;
            EmailAddress = user.Email;
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("/Account/Manage");

            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (CanDelete)
            {
                await _signInManager.SignOutAsync();
                await _userManager.DeleteAsync(user);
            }
            else
            {
                return new StatusCodeResult((int)HttpStatusCode.Forbidden);
            }

            return RedirectToPage("/Account/Login");
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            if (!ModelState.IsValid)
                return RedirectToPage("/Account/Manage");

            return RedirectToPage("/Account/Manage");
        }
    }
}