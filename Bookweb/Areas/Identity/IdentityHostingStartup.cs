using System;
using Bookweb.Areas.Identity.Data;
using Bookweb.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Bookweb.Areas.Identity.IdentityHostingStartup))]
namespace Bookweb.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<BookwebContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("BookwebContextConnection")));

                services.AddDefaultIdentity<BookwebUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<BookwebContext>();
            });
        }
    }
}