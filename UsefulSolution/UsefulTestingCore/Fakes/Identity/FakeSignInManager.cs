﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using UsefulDatabase.Model.Users;

namespace UsefulTestingCore.Fakes.Identity
{
    public class FakeSignInManager : SignInManager<User>
    {
        public FakeSignInManager()
            : base(new Mock<FakeUserManager>().Object,
                  new Mock<IHttpContextAccessor>().Object,
                  new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                  new Mock<IOptions<IdentityOptions>>().Object,
                  new Mock<ILogger<SignInManager<User>>>().Object,
                  new Mock<IAuthenticationSchemeProvider>().Object,
                  new Mock<IUserConfirmation<User>>().Object)
        { }
    }
}
