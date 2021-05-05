using HouseOfSoulSounds.Models.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using HouseOfSoulSounds.Models.Domain;
using Microsoft.AspNetCore.Identity;
using HouseOfSoulSounds.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using HouseOfSoulSounds.Models.Domain.Repositories.Abstract;
using HouseOfSoulSounds.Models.Domain.Repositories.EntityFramework;

namespace HouseOfSoulSounds
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
            Configuration.Bind("Project", new Config());
            Config.WebRootPath = Path
                .Combine(Configuration.GetValue<string>(WebHostDefaults.ContentRootKey), "wwwroot");

            //подключаем нужный функционал приложения в качестве сервиса
            services.AddTransient<ITextFieldsRepository, EFTextFieldsRepository>();
            services.AddTransient<IInstrumentsItemsRepository, EFInstrumentsItemsRepository>();
            services.AddTransient<IMessageRepository, EFMessagesRepository>();
            services.AddTransient<ICatalogsRepository, EFCatalogsRepository>();
            services.AddTransient<IPageRepository, EFPageRepository>();
            services.AddTransient<DataManager>();
            services.AddSingleton<ConnectionDictionary<string>> ();
            services.AddSignalR().AddHubOptions<ChatHub>
                (hubOptions =>
                {
                    hubOptions.EnableDetailedErrors = true;
                    hubOptions.KeepAliveInterval = System.TimeSpan.FromMinutes(120);

                });
      
            string connection = Configuration.GetConnectionString("DefaultConnection");
          services.AddDbContext<EFAppDbContext>(options => options.UseSqlServer(connection));
            //настраиваем identity систему и сложность пароля
            services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<EFAppDbContext>().AddDefaultTokenProviders();

            //настраиваем authentication cookie
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = "HouseOfSoulSoundsAuth";
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/Accessdenied";
                options.SlidingExpiration = true;
            });

            //настраиваем политику авторизации для Admin area
            //и чата
            services.AddAuthorization(x =>
            {
                x.AddPolicy("AdminArea", policy
                    => { policy.RequireRole(Config.RoleAdmin); });
            });

            //добавляем сервисы для контроллеров и представлений (MVC)

            services.AddControllersWithViews(x =>
            {
                x.Conventions.Add(new AreasAuthorization("Admin", "AdminArea"));
            })
                //выставляем совместимость с asp.net core 3.0
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0).AddSessionStateTempDataProvider();
            // установка конфигурации подключения
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options => //CookieAuthenticationOptions
            //    {
            //        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
            //    });
          //  services.AddControllersWithViews();
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
                endpoints.MapControllerRoute(Config.Admin,
                    "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
           
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default2",
            //        pattern: "{controller=Instruments}/{action=Instruments}/{id?}");
            //});
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default2",
            //        pattern: "{controller=Instruments}/{action=Instruments}/{gutar?}");
            //});
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default4",
            //        pattern: "{controller=Carts}/{action=Carts}/{id?}");
            //    endpoints.MapControllerRoute(
            //        name: "default5",
            //        pattern: "{controller=Barabans}/{action=Index}/{id?}");
            //    endpoints.MapControllerRoute(
            //        name: "default7",
            //        pattern: "{controller=Account}/{action=Login}/{id?}");

            //    endpoints.MapControllerRoute(
            //        name: "default6",
            //        pattern: "{controller=Account}/{action=Register}/{id?}");

            //    endpoints.MapControllerRoute(
            //        name: "default8",
            //        pattern: "{controller=Wind}/{action=Index}/{id?}");


            //    endpoints.MapControllerRoute(
            //        name: "default10",
            //        pattern: "{controller=Home}/{action=Info}/{id?}");


            //    endpoints.MapControllerRoute(Config.Admin,
            //        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            //    endpoints.MapControllerRoute(
            //      name: "Myroot",
            //      pattern: "{controller=CatalogsItems}/{action=Edit}/{id?}");
        //});

        }
    }
}
