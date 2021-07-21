using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyForum.Authorization;
using MyForum.Data;
using MyForum.Data.Interfaces;
using MyForum.Data.Services;
using System;
using System.Threading.Tasks;

namespace MyForum
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<MyForumDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MyForumDb"));
            });

            services.AddScoped<IGuildData, GuildData>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IMessageData, MessageData>();

            services.AddRazorPages();
            //services.AddControllers();

            services.AddSingleton<IAuthorizationHandler, YearsOfServiceAuthorizationHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsNovice", policy =>
                    policy.RequireClaim("Rank", "Novice"));
                options.AddPolicy("IsGuildmaster", policy =>
                    policy.RequireClaim("Rank", "Guildmaster"));
                options.AddPolicy("IsLeader", policy =>
                    policy.RequireClaim("Rank", "Leader"));
                options.AddPolicy("2YearsOfService",
                    policy => policy.AddRequirements(
                        new YearsOfServiceRequirement(2)
                        )
                    );
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages().RequireAuthorization();
                //endpoints.MapControllers();
            });
        }
    }
}
