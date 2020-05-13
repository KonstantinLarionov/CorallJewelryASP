using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using afc_studio.Models.Entitys;
using ChatModule;
using ChatModule.Models.Chat.Entitys;
using CorallJewelry.Entitys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CorallJewelry
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
            services.AddSignalR();
            services.AddDistributedMemoryCache();
            //services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(100000);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            services.AddMvc();
            services.AddDbContextPool<FrontendContext>(
               options => options.UseMySql("Server=localhost;Database=u0959678_coralljewelry;User=u0959_admcorall;Password=sOq2e&032;",
                   mySqlOptions =>
                   {
                       mySqlOptions.ServerVersion(new Version(5, 6, 45), ServerType.MySql);
                   }
           ));
            services.AddDbContextPool<BackendContext>(
              options => options.UseMySql("Server=localhost;Database=u0959678_coralljewelry;User=u0959_admcorall;Password=sOq2e&032;",
                  mySqlOptions =>
                  {
                      mySqlOptions.ServerVersion(new Version(5, 6, 45), ServerType.MySql);
                  }
          ));
            services.AddDbContextPool<ChatContext>(
            options => options.UseMySql("Server=localhost;Database=u0959678_onlyChat;User=u0959_admcorall;Password=sOq2e&032;",
                mySqlOptions =>
                {
                    mySqlOptions.ServerVersion(new Version(5, 6, 45), ServerType.MySql);
                }
        ));
            //  services.AddDbContextPool<MainContext>(
            //    options => options.UseMySql("Server=localhost;Database=u0959678_chatcj;User=u0959_admcorall2;Password=sOq2e&032;",
            //        mySqlOptions =>
            //        {
            //            mySqlOptions.ServerVersion(new Version(5, 6, 45), ServerType.MySql);
            //        }
            //));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
