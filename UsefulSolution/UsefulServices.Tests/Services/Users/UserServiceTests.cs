using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Threading.Tasks;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Users;
using UsefulServices.Services.Users;
using UsefulTestingCore.Fakes.Identity;
using UsefulTestingCore.Fixtures;
using Xunit;

namespace UsefulServices.Tests.Services.Users
{
    public class UserServiceTests : ServiceTests
    {
        private readonly Mock<FakeUserManager> mockUserManager;

        public UserServiceTests()
        {
            mockUserManager = new Mock<FakeUserManager>();
        }

        public UserService GetUserService(
            UsefulContext context = null,
            UserManager<User> userManager = null,
            IMemoryCache cache = null,
            IMapper mapper = null) => new UserService(
                context ?? new DatabaseFixture().Context,
                userManager ?? new Mock<FakeUserManager>().Object,
                cache ?? new Mock<IMemoryCache>().Object,
                mapper ?? new Mock<IMapper>().Object);

        [Fact]
        public async Task GetByIDAsync_ReturnsUser_WhenUserExists()
        {
            //ARRANGE
            var id = "0";
            var user = new User();
            var fixture = new DatabaseFixture();
            var userService = GetUserService(fixture.Context, mockUserManager.Object, mapper: mapper);
            mockUserManager.Setup(x => x.FindByIdAsync(id)).ReturnsAsync(user);

            //ACT
            var result = await userService.GetByIDAsync(id);

            //ASSERT
            Assert.True(result.Succeeded);
            Assert.Empty(result.Errors);
            Assert.NotNull(result.User);
        }

        [Fact]
        public async Task GetByIDAsync_ReturnsError_WhenUserDoesntExist()
        {
            //ARRANGE
            var id = "0";
            var user = new User();
            var fixture = new DatabaseFixture();
            var userService = GetUserService(fixture.Context, mockUserManager.Object, mapper: mapper);
            mockUserManager.Setup(x => x.FindByIdAsync(id)).ReturnsAsync((User)null);

            //ACT
            var result = await userService.GetByIDAsync(id);

            //ASSERT
            Assert.False(result.Succeeded);
            Assert.NotEmpty(result.Errors);
            Assert.Null(result.User);
        }
    }
}
