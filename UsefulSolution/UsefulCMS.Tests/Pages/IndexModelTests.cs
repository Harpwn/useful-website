using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsefulCMS.Pages;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fakes.Identity;
using Xunit;

namespace UsefulCMS.Tests.Pages
{
    public class IndexModelTests : CMSModelTests
    {
        private readonly Mock<FakeUserManager> mockUserManager;

        public IndexModelTests()
        {
            mockUserManager = new Mock<FakeUserManager>();
        }

        public IndexModel GetIndexModel(
            UserManager<User> userManager = null,
            IMapper mapper = null) => new IndexModel(
                userManager ?? new Mock<FakeUserManager>().Object,
                mapper ?? new Mock<IMapper>().Object);

        [Fact]
        public async Task OnGetAsync_GetsPage()
        {
            //ARRANGE
            var model = GetIndexModel(mockUserManager.Object, mapper);
            mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleType.Standard.ToString())).ReturnsAsync(new List<User> { new User() });
            mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleType.Administrator.ToString())).ReturnsAsync(new List<User> { new User(), new User() });
            mockUserManager.Setup(x => x.GetUsersInRoleAsync(RoleType.SuperAdministrator.ToString())).ReturnsAsync(new List<User> { new User(), new User(), new User() });

            //ACT
            await model.OnGetAsync();

            //ASSERT
            Assert.Equal(1, model.StandardCount);
            Assert.Equal(3, model.SuperAdminCount);
            Assert.Equal(2, model.AdminCount);
            Assert.Equal(6, model.UserCount);
        }
    }
}
