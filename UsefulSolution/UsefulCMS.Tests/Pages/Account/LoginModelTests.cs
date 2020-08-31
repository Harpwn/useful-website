using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Security.Claims;
using System.Threading.Tasks;
using UsefulCMS.Pages.Account;
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fakes.Identity;
using Xunit;

namespace UsefulCMS.Tests.Pages.Account
{
    public class LoginModelTests : CMSModelTests
    {
        private readonly Mock<FakeSignInManager> mockSignInManager;
        private readonly Mock<FakeUserManager> mockUserManager;

        public LoginModelTests()
        {
            mockSignInManager = new Mock<FakeSignInManager>();
            mockUserManager = new Mock<FakeUserManager>();
        }

        public LoginModel GetLoginModel(
            SignInManager<User> signInManager = null,
            UserManager<User> userManager = null,
            IMapper mapper = null) => new LoginModel(
                signInManager ?? new Mock<FakeSignInManager>().Object,
                userManager ?? new Mock<FakeUserManager>().Object,
                mapper ?? new Mock<IMapper>().Object);

        [Fact]
        public void OnGet_RedirectsToIndex_WhenUserIsSignedIn()
        {
            // Arrange
            var model = GetLoginModel(mockSignInManager.Object, mockUserManager.Object, mapper);
            mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(true);

            // Act
            var result = model.OnGet();

            // Arrange
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPost_RedirectsToHome_WhenUserIsAdmin()
        {
            // Arrange
            var username = "John";
            var password = "abcd1234";
            var rememberMe = true;
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            var model = GetLoginModel(mockSignInManager.Object, mockUserManager.Object, mapper);
            model.Username = username;
            model.Password = password;
            model.RememberMe = rememberMe;
            model.Url = mockUrlHelper.Object;

            var user = new User()
            {
                UserName = username,
            };

            mockUrlHelper.Setup(x => x.IsLocalUrl(string.Empty)).Returns(true);
            mockSignInManager.Setup(x => x.PasswordSignInAsync(username, password, rememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            mockUserManager.Setup(x => x.FindByNameAsync(username)).ReturnsAsync(user);

            // Act
            var result = await model.OnPostAsync();

            // Arrange
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPost_RedirectsToReturnUrl_WhenReturnUrlInModelIsValid()
        {
            // Arrange
            var username = "John";
            var password = "abcd1234";
            var returnUrl = "/game";
            var rememberMe = true;
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            var model = GetLoginModel(mockSignInManager.Object, mockUserManager.Object, mapper);
            model.Username = username;
            model.Password = password;
            model.RememberMe = rememberMe;
            model.ReturnUrl = returnUrl;
            model.Url = mockUrlHelper.Object;

            var user = new User()
            {
                UserName = username,
            };

            mockUrlHelper.Setup(x => x.IsLocalUrl(returnUrl)).Returns(true);
            mockSignInManager.Setup(x => x.PasswordSignInAsync(username, password, rememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            mockUserManager.Setup(x => x.FindByNameAsync(username)).ReturnsAsync(user);

            // Act
            var result = await model.OnPostAsync();

            // Arrange
            var redirect = Assert.IsType<RedirectResult>(result);
            Assert.Equal(model.ReturnUrl, redirect.Url);
        }

        [Fact]
        public async Task OnPost_RedirectsToHome_WhenReturnUrlInModelIsNotLocal()
        {
            // Arrange
            var username = "John";
            var password = "abcd1234";
            var returnUrl = "www.google.com";
            var rememberMe = true;
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            var model = GetLoginModel(mockSignInManager.Object, mockUserManager.Object, mapper);

            model.Username = username;
            model.Password = password;
            model.RememberMe = rememberMe;
            model.ReturnUrl = returnUrl;
            model.Url = mockUrlHelper.Object;

            var user = new User()
            {
                UserName = username,
            };

            mockUrlHelper.Setup(x => x.IsLocalUrl(returnUrl)).Returns(false);
            mockSignInManager.Setup(x => x.PasswordSignInAsync(username, password, rememberMe, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            mockUserManager.Setup(x => x.FindByNameAsync(username)).ReturnsAsync(user);

            // Act
            var result = await model.OnPostAsync();

            // Arrange
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName);
        }

        [Fact]
        public async Task OnPost_ReturnsModel_WhenInvalid()
        {
            // Arrange
            // Arrange
            var username = "John";
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            var model = GetLoginModel(mockSignInManager.Object, mockUserManager.Object, mapper);
            model.Username = username;
            model.Password = null;
            model.Url = mockUrlHelper.Object;

            mockUrlHelper.Setup(x => x.IsLocalUrl(string.Empty)).Returns(true);
            model.ModelState.AddModelError("Password", "Required");

            // Act
            var result = await model.OnPostAsync();

            // Arrange
            Assert.IsType<PageResult>(result);
            Assert.False(model.ModelState.IsValid);
        }
    }
}
