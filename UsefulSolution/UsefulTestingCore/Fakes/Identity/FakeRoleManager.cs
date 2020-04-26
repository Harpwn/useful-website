using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using UsefulDatabase.Model.Roles;

namespace UsefulTestingCore.Fakes.Identity
{
    public class FakeRoleManager : RoleManager<Role>
    {
        public FakeRoleManager()
            : base(new Mock<IRoleStore<Role>>().Object,
                  new IRoleValidator<Role>[0],
                  new Mock<ILookupNormalizer>().Object,
                  new Mock<IdentityErrorDescriber>().Object,
                  new Mock<ILogger<RoleManager<Role>>>().Object
            )
        {
        }
    }
}
