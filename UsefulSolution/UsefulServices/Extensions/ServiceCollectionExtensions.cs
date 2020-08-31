using Microsoft.Extensions.DependencyInjection;
using UsefulServices.Services.Users;
using UsefulServices.Services.Users.Moderation;

namespace UsefulServices.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserBanService, UserBanService>();
        }
    }
}
