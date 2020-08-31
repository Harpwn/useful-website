using Microsoft.Extensions.DependencyInjection;
using UsefulServices.Extensions;
using Xunit;

namespace UsefulServices.Tests.Extensions
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddCoreServices_AddsServices()
        {
            var collection = new ServiceCollection();
            var count = collection.Count;

            collection.AddCoreServices();
            Assert.True(collection.Count > count);

        }
    }
}
