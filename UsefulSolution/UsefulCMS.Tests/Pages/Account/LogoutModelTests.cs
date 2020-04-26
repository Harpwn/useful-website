using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UsefulCMS.Pages.Account;
using UsefulTestingCore.Fakes.Identity;
using Xunit;

namespace UsefulCMS.Tests.Pages.Account
{
    public class LogoutModelTests : CMSModelTest
    {
        private readonly Mock<FakeSignInManager> mockSignInManager;

        public LogoutModelTests()
        {
            mockSignInManager = new Mock<FakeSignInManager>();
        }

        [Fact]
        public void OnGet_Redirects()
        {
            //Arrange
            var model = new LogoutModel(mockSignInManager.Object, mapper);
            model.PageContext.HttpContext = new DefaultHttpContext();

            //Act
            var result = model.OnGet();

            //Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName);
        }
    }
}
