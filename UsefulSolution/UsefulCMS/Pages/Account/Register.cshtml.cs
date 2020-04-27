using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UsefulCMS.Web;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;

namespace UsefulCMS.Pages.Account
{
    public class RegisterModel : CMSPageModel
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [BindProperty]
        public string Username { get; set; }

        ///TODO: Compare attribute doesnt work in current asp.net. When v5, fix this shit
        [BindProperty]
        public ViewModel viewModel { get; set; }
        public class ViewModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare(nameof(Password))]
            [Display(Name = "Confirm Password")]
            public string ConfirmPassword { get; set; }
        }

        public RegisterModel(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = Username, Email = Username };
                var result = await _userManager.CreateAsync(user, viewModel.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, RoleType.Standard.ToString());
                    var signInResult = await _signInManager.PasswordSignInAsync(Username, viewModel.Password, false, false);
                    return RedirectToPage("/Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return Page();
        }
    }
}