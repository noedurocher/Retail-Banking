using CaseStudy.Data;
using CaseStudy.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace CaseStudy
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
            //var connection = Configuration.GetConnectionString("DefaultConnection");
            //services.AddControllers()
            //    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());              
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                   Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<ManagementContext>(c => c.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddTransient<IAccountRepo, AccountRepo>();
            services.AddTransient<ICustomerRepo, CustomerRepo>();
            services.AddTransient<ITransactionRepo, TransactionRepo>();
            services.AddTransient<IErrorRepo, ErrorRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddConsole(LogLevel.Information);
            var logger = loggerFactory.CreateLogger("Middleware");
            //app.UseCors(options => options.WithOrigins("http://localhost:5000").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
       
            CreateRoles(serviceProvider).Wait();

            app.Use(async (context, next) =>
            {
                var myTimer = System.Diagnostics.Stopwatch.StartNew();
                logger.LogInformation($"Beginning request in {env.EnvironmentName} environment");

                await next();
                logger.LogInformation($"Request completed in {myTimer.ElapsedMilliseconds}ms");
            });

            app.Run(async (context) =>
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Message displayed by middleware!");
            });
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string[] roleNames = { "CustomerExecutive", "AccountExecutive", "Cashier", "Teller","Admin" };
            IdentityResult roleResult;
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var _admin = await UserManager.FindByEmailAsync("admin@admin.com");
            if (_admin == null)
            {
                var admin = new IdentityUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    EmailConfirmed = true
                };
                var createAdmin = await UserManager.CreateAsync(admin, "Admin2019!");
                if (createAdmin.Succeeded)
                    await UserManager.AddToRoleAsync(admin, "Admin");
            }

        }
    }
}
