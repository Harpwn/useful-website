using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using UsefulCMS.Models;
using UsefulDatabase.Model.Users;

namespace UsefulCMS.Pages.Account
{
    public class ChangePasswordModel : AuthorizedCMSPageModel
    {
        private UserManager<User> _userManager;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        [BindProperty]
        public string OldPassword { get; set; }

        [BindProperty]
        public PasswordConfirmViewModel NewPasswordModel { get; set; }

        public class PasswordConfirmViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare(nameof(Password))]
            public string PasswordConfirm { get; set; }
        }

        

        public ChangePasswordModel(UserManager<User> userManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            if (!await _userManager.CheckPasswordAsync(user, OldPassword))
                ModelState.AddModelError("OldPassword", "Incorrect Password");

            if (!ModelState.IsValid)
                return Page();

            var result = await _userManager.ChangePasswordAsync(user, OldPassword, NewPasswordModel.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return Page();
            }


            return RedirectToPage("/Account/Manage");
        }
    }
}