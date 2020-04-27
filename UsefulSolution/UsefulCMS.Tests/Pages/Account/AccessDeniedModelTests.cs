using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using UsefulCMS.Pages.Account;
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

        [Fact]
        public void OnGet_RedirectsToLogin_WhenUserIsNotSignedIn()
        {
            // Arrange
            var model = new AccessDeniedModel(mockSignInManager.Object, mapper);
            mockSignInManager.Setup(x => x.IsSignedIn(It.IsAny<ClaimsPrincipal>())).Returns(false);

            // Act
            var result = model.OnGet();

            // Arrange
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Account/Login", redirectToPageResult.PageName);
        }

    }
}
