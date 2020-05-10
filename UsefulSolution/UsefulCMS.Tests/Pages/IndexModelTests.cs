using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UsefulCMS.Pages;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Users;
using UsefulTestingCore.Fakes.Identity;
using UsefulTestingCore.Fixtures;
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
