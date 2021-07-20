using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyForum.Core;
using MyForum.Data;

[assembly: HostingStartup(typeof(MyForum.Areas.Identity.IdentityHostingStartup))]
namespace MyForum.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => 
            {
                services.AddIdentity<MyUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddRoles<IdentityRole>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddUserManager<UserManager<MyUser>>()
                    .AddEntityFrameworkStores<MyForumDbContext>()
                    .AddDefaultTokenProviders();
                    //.AddDefaultUI();

                services.AddScoped<IUserClaimsPrincipalFactory<MyUser>, ApplicationUserClaimsPrincipalFactory>();

                services.ConfigureApplicationCookie(options => {
                    options.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                    options.LoginPath = new PathString("/Identity/Account/Login");
                    options.LogoutPath = new PathString("/Identity/Account/Logout");
                });
            });
        }
    }
}