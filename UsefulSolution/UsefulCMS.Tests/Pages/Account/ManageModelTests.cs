using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    public class ManageModelTests : CMSModelTests
    {
        private readonly Mock<FakeSignInManager> mockSignInManager;
        private readonly Mock<FakeUserManager> mockUserManager;

        public ManageModelTests()
        {
            mockSignInManager = new Mock<FakeSignInManager>();
            mockUserManager = new Mock<FakeUserManager>();
        }

        public ManageModel GetManageModel(
            UserManager<User> userManager = null,
            SignInManager<User> signInManager = null,
            IMapper mapper = null) => new ManageModel(
                userManager ?? new Mock<FakeUserManager>().Object,
                signInManager ?? new Mock<FakeSignInManager>().Object,
                mapper ?? new Mock<IMapper>().Object);

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
            var model = GetManageModel(mockUserManager.Object, mockSignInManager.Object, mapper);
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
            var model = GetManageModel(mockUserManager.Object, mockSignInManager.Object, mapper);
            model.Username = "SuperAdmin";
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
            var model = GetManageModel(mockUserManager.Object, mockSignInManager.Object, mapper);
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
            var model = GetManageModel(mockUserManager.Object, mockSignInManager.Object, mapper);
            model.Username = username;
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

        
    }
}
