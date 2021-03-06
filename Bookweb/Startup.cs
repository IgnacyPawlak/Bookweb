using Bookweb.Areas.Identity.Data;
using Bookweb.Services;
using Bookweb.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bookweb
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
            services.AddControllersWithViews();
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddTransient<IMailService, Services.MailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();
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

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            updateUsers(app.ApplicationServices);
        }
        private async void updateUsers(IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var userManager = (UserManager<BookwebUser>)scope.ServiceProvider.GetService(typeof(UserManager<BookwebUser>));
                var user = await userManager.FindByEmailAsync("user@example.com");
                if (user != null)
                {
                    user.EmailConfirmed = true;
                    await userManager.UpdateAsync(user);
                }

                var admin = await userManager.FindByEmailAsync("admin@example.com");
                if (admin != null)
                {
                    admin.EmailConfirmed = true;
                    await userManager.UpdateAsync(admin);
                }

                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
                if(!await roleManager.RoleExistsAsync("ADMIN"))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = "ADMIN" });
                }

                if (!await roleManager.RoleExistsAsync("USER"))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = "USER" });
                }

                admin = await userManager.FindByEmailAsync("admin@example.com");

                if (admin!=null && !await userManager.IsInRoleAsync(admin, "ADMIN"))
                {
                    await userManager.RemoveFromRoleAsync(admin, "USER");
                    await userManager.AddToRoleAsync(admin, "ADMIN");
                    await userManager.UpdateAsync(admin);
                }

                user = await userManager.FindByEmailAsync("user@example.com");

                if (user != null &&!await userManager.IsInRoleAsync(user, "USER"))
                {
                    await userManager.AddToRoleAsync(user, "USER");
                    await userManager.UpdateAsync(user);
                }


               
            }
        }
    }
}
