using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Threading.Tasks;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Users;
using UsefulServices.Services.Users.Moderation;
using UsefulTestingCore.Fakes.Identity;
using UsefulTestingCore.Fixtures;
using Xunit;

namespace UsefulServices.Tests.Services.Users.Moderation
{
    public class UserBanServiceTests
    {
        private readonly Mock<FakeUserManager> mockUserManager;

        public UserBanServiceTests()
        {
            mockUserManager = new Mock<FakeUserManager>();
        }

        public UserBanService Service(
            UsefulContext context = null,
            UserManager<User> userManager = null,
            IMemoryCache cache = null,
            IMapper mapper = null) => new UserBanService(
                context ?? new DatabaseFixture().Context,
                userManager ?? new Mock<FakeUserManager>().Object);

        [Fact]
        public async Task BanUser_BansUser()
        {
            //ARRANGE
            var id = 0;
            var banDuration = new TimeSpan(6, 6, 6);
            var bannedReason = "test";
            var user = new User();
            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync(user);

            var result = await Service(userManager: mockUserManager.Object).BanUser(id, bannedReason, banDuration);

            Assert.True(result.Succeeded);
            Assert.True(user.IsBanned);
            Assert.True(user.BannedUntilDate > DateTime.UtcNow);
            Assert.Equal(bannedReason, user.BannedReason);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }

        [Fact]
        public async Task BanUser_DoesntBanUser_WhenDoesntExist()
        {
            var id = 0;
            var banDuration = new TimeSpan(6, 6, 6);
            var bannedReason = "test";
            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync((User)null);

            var result = await Service(userManager: mockUserManager.Object).BanUser(id, bannedReason, banDuration);

            Assert.False(result.Succeeded);
            mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UnBanUser_DoesntUnBanUser_WhenDoesntExist()
        {
            var id = 0;
            var banDuration = new TimeSpan(6, 6, 6);
            var bannedReason = "test";
            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync((User)null);

            var result = await Service(userManager: mockUserManager.Object).UnbanUser(id);

            Assert.False(result.Succeeded);
            mockUserManager.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task UnBanUser_UnBansUser()
        {
            //ARRANGE
            var id = 0;
            var user = new User();
            mockUserManager.Setup(x => x.FindByIdAsync(id.ToString())).ReturnsAsync(user);

            var result = await Service(userManager: mockUserManager.Object).UnbanUser(id);

            Assert.True(result.Succeeded);
            Assert.False(user.IsBanned);
            Assert.Null(user.BannedUntilDate);
            Assert.Equal(string.Empty, user.BannedReason);
            mockUserManager.Verify(x => x.UpdateAsync(user), Times.Once);
        }
    }
}
