using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class ChangePasswordModelTests : CMSModelTests
    {
        private readonly Mock<FakeUserManager> mockUserManager;

        public ChangePasswordModelTests()
        {
            mockUserManager = new Mock<FakeUserManager>();
        }

        public ChangePasswordModel GetModel(
            UserManager<User> userManager = null,
            IMapper mapper = null) => new ChangePasswordModel(
                userManager ?? new Mock<FakeUserManager>().Object,
                mapper ?? new Mock<IMapper>().Object);


        [Fact]
        public async Task OnPostChangePasswordAsync_WhenValid_Redirects()
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
            var model = GetModel(mockUserManager.Object, mapper);

            model.OldPassword = oldPassword;
            model.NewPasswordModel = new ChangePasswordModel.PasswordConfirmViewModel
            {
                Password = newPassword,
                PasswordConfirm = newPassword
            };

            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, oldPassword)).ReturnsAsync(true);
            mockUserManager.Setup(x => x.ChangePasswordAsync(user, oldPassword, newPassword)).ReturnsAsync(IdentityResult.Success);
            // Act
            var result = await model.OnPostAsync();

            // Arrange
            Assert.True(model.ModelState.IsValid);
            Assert.IsType<RedirectToPageResult>(result);
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
            var model = GetModel(mockUserManager.Object, mapper);
            model.OldPassword = oldPassword;
            model.NewPasswordModel = new ChangePasswordModel.PasswordConfirmViewModel
            {
                Password = newPassword,
                PasswordConfirm = null
            };

            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, oldPassword)).ReturnsAsync(true);
            model.ModelState.AddModelError("NewPasswordModel.PasswordConfirm", "Required");

            // Act
            var result = await model.OnPostAsync();

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
            var model = GetModel(mockUserManager.Object, mapper);
            model.OldPassword = oldPassword;
            model.NewPasswordModel = new ChangePasswordModel.PasswordConfirmViewModel
            {
                Password = newPassword,
                PasswordConfirm = newPassword
            };

            model.PageContext.HttpContext = new DefaultHttpContext();
            mockUserManager.Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            mockUserManager.Setup(x => x.CheckPasswordAsync(user, oldPassword)).ReturnsAsync(false);

            // Act
            var result = await model.OnPostAsync();

            // Arrange
            Assert.False(model.ModelState.IsValid);
            Assert.IsType<PageResult>(result);
        }
    }
}
