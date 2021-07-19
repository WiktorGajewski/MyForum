using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                    .AddUserManager<UserManager<MyUser>>()
                    .AddEntityFrameworkStores<MyForumDbContext>();
                    //.AddDefaultUI();
                    //.AddDefaultTokenProviders();

                services.AddScoped<IUserClaimsPrincipalFactory<MyUser>, ApplicationUserClaimsPrincipalFactory>();
            });
        }
    }
}