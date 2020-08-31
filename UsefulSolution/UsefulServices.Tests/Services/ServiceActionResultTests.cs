using Microsoft.AspNetCore.Identity;
using System.Linq;
using UsefulServices.Services;
using Xunit;

namespace UsefulServices.Tests.Services
{
    public class ServiceActionResultTests
    {
        [Fact]
        public void ServiceActionResult_Succeeded_WhenNoError()
        {
            var result = new ServiceActionResult();
            Assert.True(result.Succeeded);
        }

        [Fact]
        public void ServiceActionResult_Failed_WhenError()
        {
            var result = new ServiceActionResult("error");
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void ServiceActionResult_Errors_FromIdentityResult()
        {
            var identityResult = IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = "error1" }, new IdentityError() { Description = "error2" } });
            var result = new ServiceActionResult(identityResult);

            Assert.Equal(2, result.Errors.ToList().Count);
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void ServiceActionResult_GetErrorMessage_ReturnsErrors()
        {
            var identityResult = IdentityResult.Failed(new IdentityError[] { new IdentityError() { Description = "error1" }, new IdentityError() { Description = "error2" } });
            var result = new ServiceActionResult(identityResult).GetErrorsMessage();

            Assert.Contains("error1", result);
            Assert.Contains("error2", result);
        }

    }
}
