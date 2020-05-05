using Microsoft.Extensions.DependencyInjection;
using UsefulServices.Services.Users;

namespace UsefulServices.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }
    }
}
