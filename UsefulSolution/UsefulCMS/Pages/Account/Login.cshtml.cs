﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using UsefulCMS.Models;
using UsefulDatabase.Model.Users;

namespace UsefulCMS.Pages.Account
{
    public class LoginModel : CMSPageModel
    {
        private SignInManager<User> _signInManager;
        private UserManager<User> _userManager;

        [Required]
        [MaxLength(50)]
        [BindProperty]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [BindProperty]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Remember Me")]
        [BindProperty]
        public bool RememberMe { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; }

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, IMapper mapper) : base(mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult OnGet(string returnUrl = "")
        {
            if (_signInManager.IsSignedIn(User))
                return RedirectToPage("/Index");

            ReturnUrl = returnUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(Username);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(Username, Password, RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToPage("/Index");
                        }
                    }
                }
            }

            ModelState.AddModelError("", "Sign In Failed");
            return Page();
        }
    }
}