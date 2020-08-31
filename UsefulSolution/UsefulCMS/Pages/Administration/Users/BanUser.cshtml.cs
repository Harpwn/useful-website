using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UsefulCMS.Models;
using UsefulCore.Enums.Moderation;
using UsefulDatabase.Model.Users;
using UsefulServices.Services.Users.Moderation;

namespace UsefulCMS.Pages.Administration.Users
{
    public class BanUserModel : AuthorizedCMSPageModel
    {
        private IUserBanService _userBanService;
        private UserManager<User> _userManager;

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [BindProperty]
        [Display(Name = "Reason for Ban")]
        public string BanReason { get; set; }

        [Required]
        [BindProperty]
        [Display(Name = "Duration of Ban")]
        public UserBanDuration BanDuration { get; set; }

        public BanUserModel(UserManager<User> userManager, IUserBanService userBanService, IMapper mapper) : base(mapper)
        {
            _userBanService = userBanService;
            _userManager = userManager;
        }

        public async Task OnGetAsync([FromQuery]int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
                RedirectToPage("/Administration/Users/Index");

            Id = id;
            UserName = user.UserName;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userBanService.BanUser(Id, BanReason, BanDuration.ToTimeSpan());
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("BanModel", error);
                    }
                }
            }

            return RedirectToPage("/Administration/Users/Details", new { id=Id });
        }
    }
}