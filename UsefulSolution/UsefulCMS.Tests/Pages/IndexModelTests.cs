using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UsefulCMS.Pages;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Roles;
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fixtures;
using Xunit;

namespace UsefulCMS.Tests.Pages
{
    public class IndexModelTests : CMSModelTests
    {
        public IndexModel GetIndexModel(
            UsefulContext context = null,
            IMapper mapper = null) => new IndexModel(
                mapper ?? new Mock<IMapper>().Object,
                context ?? new DatabaseFixture().Context);

        [Fact]
        public async Task OnGetAsync_GetsPage()
        {
            //ARRANGE
            var fixture = new DatabaseFixture();
            var model = GetIndexModel(fixture.Context, mapper);
            fixture.Context.Users.AddRange(new List<User>
            {
                new User { Roles = new List<UserRole> { new UserRole { Role = new Role { Name = RoleType.Standard.ToString() } } } },
                new User { Roles = new List<UserRole> { new UserRole { Role = new Role { Name = RoleType.SuperAdministrator.ToString() } } } },
                new User { Roles = new List<UserRole> { new UserRole { Role = new Role { Name = RoleType.Administrator.ToString() } } } },
            });
            fixture.Context.SaveChanges();

            //ACT
            await model.OnGetAsync();

            //ASSERT
            Assert.Equal(1, model.StandardCount);
            Assert.Equal(1, model.SuperAdminCount);
            Assert.Equal(1, model.AdminCount);
            Assert.Equal(3, model.UserCount);
        }
    }
}
