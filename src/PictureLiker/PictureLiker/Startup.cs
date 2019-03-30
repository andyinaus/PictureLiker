using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PictureLiker.DAL;
using PictureLiker.DAL.Repositories;
using PictureLiker.Extensions;
using PictureLiker.Factories;
using PictureLiker.Services;

namespace PictureLiker
{
    public class Startup
    {
        public const string DefaultAuthenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        private const string DefaultConnectionStringName = "PictureLikerDB";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            }); 

            services.AddDbContext<PictureLikerContext>(o => o.UseSqlServer(Configuration.GetConnectionString(DefaultConnectionStringName)));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IDomainQuery), typeof(DomainQuery));
            services.AddScoped(typeof(IEntityFactory), typeof(EntityFactory));

            services.AddAuthentication(DefaultAuthenticationScheme)
                .AddCookie(DefaultAuthenticationScheme, options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                    options.LoginPath = "/Account/Login";
                    options.SlidingExpiration = true;
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            AddAdminUserIfNotExist(app);

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

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private static void AddAdminUserIfNotExist(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                var factory = scope.ServiceProvider.GetService<IEntityFactory>();

                if (unitOfWork.UseRepository.FirstOrDefault(
                        u => u.Role.EqualsIgnoreCase(Authentication.RoleTypes.Administrator)) == null)
                {
                    var adminUser = factory.GetUser()
                        .SetName("Admin")
                        .SetEmail("admin@gmail.com").Result
                        .SetRole(Authentication.RoleTypes.Administrator);

                    unitOfWork.UseRepository.Add(adminUser);
                    unitOfWork.Save();
                }
            }
        }
    }
}
