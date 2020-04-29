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
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fakes.Identity;
using Xunit;

namespace UsefulCMS.Tests.Pages.Account
{
    public class ManageModelTests : CMSModelTests
    {
        private readonly Mock<FakeSignInManager> mockSignInManager;
        private readonly Mock<FakeUserManager> mockUserManager;

        public ManageModelTests()
        {
            mockSignInManager = new Mock<FakeSignInManager>();
            mockUserManager = new Mock<FakeUserManager>();
        }

        [Fact]
        public async Task OnGetAsync_GetsPage()
        {
            //Arrange
            var username = "test";
            var email = "test@test.test";
            var user = new User
            {
                UserName = username,
                Email = email
            };
            var model = new ManageModel(mockUserManager.Object, mockSignInManager.Object, mapper);
            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

            //Act
            await model.OnGetAsync();

            //Assert
            Assert.Equal(username, model.Username);
            Assert.Equal(email, model.EmailAddress);
        }

        [Fact]
        public async Task OnPostDeleteAsync_ReturnsForbidden_WhenUserIsSuperAdmin()
        {
            // Arrange
            var model = new ManageModel(mockUserManager.Object, mockSignInManager.Object, mapper)
            {
                Username = "SuperAdmin"
            };
            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            // Act
            var result = await model.OnPostDeleteAsync();

            // Arrange
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(403, statusCodeResult.StatusCode);
        }

        [Fact]
        public async Task OnPostDeleteAsync_Redirects_WhenNotValid()
        {
            // Arrange
            var model = new ManageModel(mockUserManager.Object, mockSignInManager.Object, mapper);
            model.ModelState.AddModelError("Username", "Required");
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new User());

            // Act
            var result = await model.OnPostDeleteAsync();

            // Arrange
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostDeleteAsync_Success_WhenUserIsNotSuperAdmin()
        {
            // Arrange
            var username = "test";
            var user = new User
            {
                UserName = username
            };
            var model = new ManageModel(mockUserManager.Object, mockSignInManager.Object, mapper)
            {
                Username = username
            };
            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockSignInManager.Setup(x => x.SignOutAsync());
            mockUserManager.Setup(x => x.DeleteAsync(user));

            // Act
            var result = await model.OnPostDeleteAsync();

            // Arrange
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Account/Login", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPostChangePasswordAsync_WhenValid_ReturnPage()
        {
            // Arrange
            var username = "test";
            var email = "test@test.test";
            var oldPassword = "password1";
            var newPassword = "password2";
            var user = new User
            {
                UserName = username,
                Email = email
            };
            var model = new ManageModel(mockUserManager.Object, mockSignInManager.Object, mapper)
            {
                Username = username,
                EmailAddress = email,
                ChangePasswordInput = new ManageModel.ChangePasswordInputModel
                {
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    NewPasswordConfirm = newPassword
                }
            };
            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, oldPassword)).ReturnsAsync(true);
            mockUserManager.Setup(x => x.ChangePasswordAsync(user, oldPassword, newPassword)).ReturnsAsync(Microsoft.AspNetCore.Identity.IdentityResult.Success);
            // Act
            var result = await model.OnPostChangePasswordAsync();

            // Arrange
            Assert.True(model.ModelState.IsValid);
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostChangePasswordAsync_WhenInvalid_ReturnsPage()
        {
            // Arrange
            var username = "test";
            var email = "test@test.test";
            var oldPassword = "password1";
            var newPassword = "password2";
            var user = new User
            {
                UserName = username,
                Email = email
            };
            var model = new ManageModel(mockUserManager.Object, mockSignInManager.Object, mapper)
            {
                Username = username,
                EmailAddress = email,
                ChangePasswordInput = new ManageModel.ChangePasswordInputModel
                {
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    NewPasswordConfirm = null
                }
            };
            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, oldPassword)).ReturnsAsync(true);
            model.ModelState.AddModelError("ChangePasswordInput.NewPasswordConfirm", "Required");

            // Act
            var result = await model.OnPostChangePasswordAsync();

            // Arrange
            Assert.False(model.ModelState.IsValid);
            Assert.IsType<PageResult>(result);
        }

        [Fact]
        public async Task OnPostChangePasswordAsync_WhenOldPasswordIncorrect_ReturnPage()
        {
            // Arrange
            var username = "test";
            var email = "test@test.test";
            var oldPassword = "password1";
            var newPassword = "password2";
            var user = new User
            {
                UserName = username,
                Email = email
            };
            var model = new ManageModel(mockUserManager.Object, mockSignInManager.Object, mapper)
            {
                Username = username,
                EmailAddress = email,
                ChangePasswordInput = new ManageModel.ChangePasswordInputModel
                {
                    OldPassword = oldPassword,
                    NewPassword = newPassword,
                    NewPasswordConfirm = newPassword
                }
            };
            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, oldPassword)).ReturnsAsync(false);

            // Act
            var result = await model.OnPostChangePasswordAsync();

            // Arrange
            Assert.False(model.ModelState.IsValid);
            Assert.IsType<PageResult>(result);
        }
    }
}
