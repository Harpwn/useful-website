using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using System.Security.Claims;
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

        public RegisterModel GetRegisterModel(
            SignInManager<User> signInManager = null,
            UserManager<User> userManager = null,
            IMapper mapper = null) => new RegisterModel(
                signInManager ?? new Mock<FakeSignInManager>().Object,
                userManager ?? new Mock<FakeUserManager>().Object,
                mapper ?? new Mock<IMapper>().Object);

        [Fact]
        public void OnGet_RedirectsToHome_WhenUserIsSignedIn()
        {
            //Arrange
            var model = GetRegisterModel(mockSignInManager.Object, mockUserManager.Object, mapper);
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
            var model = GetRegisterModel(mockSignInManager.Object, mockUserManager.Object, mapper);
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
            var model = GetRegisterModel(mockSignInManager.Object, mockUserManager.Object, mapper);
            model.Username = username;
            model.viewModel = new RegisterModel.ViewModel
            {
                Password = password,
                ConfirmPassword = password
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
