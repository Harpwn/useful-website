using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using UsefulCMS.Pages.Account;
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fakes.Identity;
using Xunit;

namespace UsefulCMS.Tests.Pages.Account
{
    public class AccessDeniedModelTests : CMSModelTests
    {
        private readonly Mock<FakeSignInManager> mockSignInManager;

        public AccessDeniedModelTests()
        {
            mockSignInManager = new Mock<FakeSignInManager>();
        }

        public AccessDeniedModel GetAccessDeniedModel(
            SignInManager<User> signInManager = null,
            IMapper mapper = null) => new AccessDeniedModel(
                signInManager ?? new Mock<FakeSignInManager>().Object,
                mapper ?? new Mock<IMapper>().Object);

        [Fact]
        public void OnGet_RedirectsToLogin_WhenUserIsNotSignedIn()
        {
            // Arrange
            var model = GetAccessDeniedModel(mockSignInManager.Object, mapper);
            mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

            // Act
            var result = model.OnGet();

            // Arrange
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Account/Login", redirectToPageResult.PageName);
        }

    }
}
