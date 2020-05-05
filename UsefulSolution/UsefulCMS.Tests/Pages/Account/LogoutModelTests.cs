using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using UsefulCMS.Pages.Account;
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fakes.Identity;
using Xunit;

namespace UsefulCMS.Tests.Pages.Account
{
    public class LogoutModelTests : CMSModelTests
    {
        private readonly Mock<FakeSignInManager> mockSignInManager;

        public LogoutModelTests()
        {
            mockSignInManager = new Mock<FakeSignInManager>();
        }

        public LogoutModel GetLogoutModel(
            SignInManager<User> signInManager = null,
            IMapper mapper = null) => new LogoutModel(
                signInManager ?? new Mock<FakeSignInManager>().Object,
                mapper ?? new Mock<IMapper>().Object);

        [Fact]
        public void OnGet_Redirects()
        {
            //Arrange
            var model = GetLogoutModel(mockSignInManager.Object, mapper);
            model.PageContext.HttpContext = new DefaultHttpContext();

            //Act
            var result = model.OnGet();

            //Assert
            var redirectToPageResult = Assert.IsType<RedirectToPageResult>(result);
            Assert.Equal("/Index", redirectToPageResult.PageName);
        }
    }
}
