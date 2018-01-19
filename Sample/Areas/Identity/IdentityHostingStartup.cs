using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sample.Areas.Identity.Services;

[assembly: HostingStartup(typeof(Sample.Areas.Identity.IdentityStartup))]
namespace Sample.Areas.Identity
{
    public class IdentityStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddTransient<IEmailSender, EmailSender>();

                services.AddDbContext<IdentityDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("IdentityDbContextConnection"),
                        sqlOptions => sqlOptions.MigrationsAssembly("Sample")));
                services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<IdentityDbContext>()
                    .AddDefaultTokenProviders();

                services.AddMvc()
                    .AddRazorPagesOptions(options =>
                    {
                        options.AllowAreas = true;
                        options.Conventions.AuthorizeFolder("/Account/Manage");
                        options.Conventions.AuthorizePage("/Account/Logout");
                    });

                services.ConfigureApplicationCookie(options => 
                {
                    options.LoginPath = "/Identity/Account/Login";
                    options.LogoutPath = "/Identity/Account/Logout";
                    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                });
            });
        }
    }
}