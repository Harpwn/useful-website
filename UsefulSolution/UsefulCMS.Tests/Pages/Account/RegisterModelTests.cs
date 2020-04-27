﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UsefulCMS.Pages.Account;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fakes.Identity;
using Xunit;

namespace UsefulCMS.Tests.Pages.Account
{
    public class RegisterModelTests : CMSModelTests
    {
        private readonly Mock<FakeSignInManager> mockSignInManager;
        private readonly Mock<FakeUserManager> mockUserManager;

        public RegisterModelTests()
        {
            mockSignInManager = new Mock<FakeSignInManager>();
            mockUserManager = new Mock<FakeUserManager>();
        }

        [Fact]
        public void OnGet_RedirectsToHome_WhenUserIsSignedIn()
        {
            //Arrange
            var model = new RegisterModel(mockSignInManager.Object, mockUserManager.Object, mapper);
            model.PageContext.HttpContext = new DefaultHttpContext();
            mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            // Act
            var result = model.OnGet();

            //Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPost_ReturnsModel_WhenInvalid()
        {
            // Arrange
            var model = new RegisterModel(mockSignInManager.Object, mockUserManager.Object, mapper);
            model.ModelState.AddModelError("Password", "Required");

            // Act
            var result = await model.OnPostAsync();

            // Arrange
            Assert.IsType<PageResult>(result);
            Assert.False(model.ModelState.IsValid);
        }

        [Fact]
        public async Task OnPost_RedirectToIndex_WhenValid()
        {
            // Arrange
            var username = "test";
            var password = "abcd1234";
            var model = new RegisterModel(mockSignInManager.Object, mockUserManager.Object, mapper)
            {
                Username = username,
                viewModel = new RegisterModel.ViewModel
                {
                    Password = password,
                    ConfirmPassword = password
                }
            };
            var user = new User
            {
                UserName = username,
                Email = username
            };

            mockUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), password)).ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
            mockUserManager.Setup(x => x.AddToRoleAsync(user, RoleType.Standard.ToString()));
            mockSignInManager.Setup(x => x.PasswordSignInAsync(username, password, false, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await model.OnPostAsync();

            // Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName);
        }
    }
}