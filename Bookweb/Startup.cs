using Bookweb.Areas.Identity.Data;
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
                var user = await userManager.FindByEmailAsync("test@test.com");
                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);


                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
                if(!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = "ADMIN" });
                }

                if (!await roleManager.RoleExistsAsync("USER"))
                {
                    await roleManager.CreateAsync(new IdentityRole { Name = "USER" });
                }

                if (!await userManager.IsInRoleAsync(user, "ADMIN"))
                {
                    await userManager.AddToRoleAsync(user, "ADMIN");
                    await userManager.UpdateAsync(user);
                }

                user = await userManager.FindByEmailAsync("test@test.com");

                if (!await userManager.IsInRoleAsync(user, "USER"))
                {
                    await userManager.AddToRoleAsync(user, "USER");
                    await userManager.UpdateAsync(user);
                }
            }
        }
    }
}
