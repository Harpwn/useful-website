using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Roles;
using UsefulDatabase.Model.Users;
using static UsefulServices.Extensions.ServiceCollectionExtensions;
using static UsefulDatabase.Seeding.Seeding;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace UsefulCMS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddDbContext<UsefulContext>(options => options.UseSqlServer(Configuration["database:connection"], b => b.MigrationsAssembly("UsefulDatabase")));
            services.AddSingleton(provider => Configuration);
            services.AddIdentity<User, Role>()
                .AddUserStore<UserStore<User, Role, UsefulContext, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityUserToken<int>, IdentityRoleClaim<int>>>()
                .AddRoleStore<RoleStore<Role, UsefulContext, int, UserRole, IdentityRoleClaim<int>>>()
                .AddDefaultTokenProviders();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddCoreServices();
            services.AddMemoryCache();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            IdentitySeeding(userManager, roleManager);

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });

            app.UseStatusCodePages("text/plain", "Status code page, status code: {0}");
        }
    }
}
